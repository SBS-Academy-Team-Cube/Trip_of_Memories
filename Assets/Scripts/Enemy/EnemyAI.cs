using UnityEngine;
using UnityEngine.AI;

public enum EState { Idle, Patrolling, Chasing, AttackWaiting, Attacking, Stunned, Death, None };

public class EnemyAI : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int WalkSpeedMultiplierHash = Animator.StringToHash("WalkSpeedMultiplier");
    private static readonly int DoAttackHash = Animator.StringToHash("DoAttack");
    private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    private static readonly int DeathTriggerHash = Animator.StringToHash("DeathTrigger");

    private EState CurrentState = EState.None;

    [SerializeField] private float PatrolSpeed, ChaseSpeed;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator AnimController;
    [SerializeField] private EnemyDetecter Detecter;
    [SerializeField] private EnemyAttacker Attacker;
    [SerializeField] private Health HP;

    [SerializeField] private AudioSource Audio;
    [SerializeField] private DistanceGatedAudioSource GatedAudio;
    [SerializeField] private RandomSoundQueue Sounds;

    private RandomSoundQueue RuntimeSounds;

    [SerializeField] private float PatrolRadius = 5f;
    [SerializeField] private float PatrolTimeMin = 1.5f;
    [SerializeField] private float PatrolTimeMax = 5f;
    [SerializeField] private float AttackableNavMeshDistance = 1f;

    private Transform PlayerTransform;
    private Vector3 LastReachablePlayerPosition;
    private float IdleInterval;
    private float IdleTimer = 0.0f;
    private bool HasFocus;
    private EState StateBeforeStun = EState.Idle;

    void Awake()
    {
        if (Agent == null)
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        if (AnimController == null)
        {
            AnimController = GetComponentInChildren<Animator>();
        }
        if (Detecter == null)
        {
            Detecter = GetComponentInChildren<EnemyDetecter>();
        }
        if (Attacker == null)
        {
            Attacker = GetComponentInChildren<EnemyAttacker>();
        }
        if (HP == null)
        {
            HP = GetComponent<Health>();
        }
        if (Audio == null)
        {
            Audio = GetComponent<AudioSource>();
        }
        if (GatedAudio == null)
        {
            GatedAudio = GetComponent<DistanceGatedAudioSource>();
        }
        if (Sounds != null)
        {
            RuntimeSounds = Instantiate(Sounds);
        }
        PatrolRadius *= transform.lossyScale.x;
    }
    private void OnEnable()
    {
        if (Detecter != null)
        {
            Detecter.OnPlayerDetected += OnPlayerFound;
        }
        if (HP != null)
        {
            HP.OnHPChanged += OnTakeDamage;
            HP.OnDead += OnDead;
        }
    }
    private void OnDisable()
    {
        if (Detecter != null)
        {
            Detecter.OnPlayerDetected -= OnPlayerFound;
        }
        if (HP != null)
        {
            HP.OnHPChanged -= OnTakeDamage;
            HP.OnDead -= OnDead;
        }
    }
    private void OnDead()
    {
        SetState(EState.Death);
    }
    private void OnTakeDamage(int HP)
    {
        if (HP <= 0 || CurrentState == EState.Death)
        {
            return;
        }
        StateBeforeStun = CurrentState;
        SetState(EState.Stunned);
    }
    public void StartAttack()
    {
        Attacker?.BeginAttack();
    }
    public void EndAttack()
    {
        Attacker?.EndAttack();
    }
    public void OnAttackAnimationEnd()
    {
        if (CurrentState != EState.Attacking)
        {
            return;
        }
        EndAttack();
        EnterPostAttackState();
    }
    public void OnStunnedAnimationEnd()
    {
        if (CurrentState != EState.Stunned)
        {
            return;
        }
        ResumeAfterStun();
    }
    public void OnDeathAnimationEnd()
    {
        if (CurrentState != EState.Death)
        {
            return;
        }

        Destroy(gameObject);
    }
    private void OnPlayerFound(bool bFound, Transform TargetPlayer)
    {
        if (bFound)
        {
            PlayerTransform = TargetPlayer;
            TryAcquireFocus();
        }
        else
        {
            PlayerTransform = null;
            ReleaseFocus();
            if (CurrentState != EState.Death)
            {
                SetState(EState.Idle);
            }
        }
    }
    void Start()
    {
        SetState(EState.Idle);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PatrolRadius);
    }

    void Update()
    {
        if (CurrentState == EState.Death)
        {
            return;
        }
        UpdateTargetState();

        switch (CurrentState)
        {
            case EState.Idle:
                IdleTimer += Time.deltaTime;
                if (IdleTimer >= IdleInterval)
                {
                    SetState(EState.Patrolling);
                }
                return;
            case EState.Patrolling:
                if (HasArrived())
                {
                    SetState(EState.Idle);
                }
                return;
            case EState.Chasing:
                if (Attacker != null && Attacker.CanAttack())
                {
                    SetState(EState.Attacking);
                }
                else
                {
                    ChaseToReachablePlayerPosition();
                }
                return;
            case EState.AttackWaiting:
                if (HasArrived())
                {
                    StopAgent();
                    AnimController.SetBool(IsMovingHash, false);
                    LookAtPlayer();
                }
                return;
            default:
                break;
        }
    }
    private void LookAtPlayer()
    {
        if (PlayerTransform == null)
        {
            return;
        }
        Vector3 Direction = PlayerTransform.position - transform.position;
        Direction.y = 0f;
        if (Direction.sqrMagnitude <= 0.001f)
        {
            return;
        }
        Quaternion TargetRotation = Quaternion.LookRotation(Direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            TargetRotation,
            Agent.angularSpeed * Time.deltaTime
        );
    }
    private void UpdateTargetState()
    {
        if (Detecter == null)
        {
            return;
        }
        if (!Detecter.IsDetected)
        {
            if (HasFocus)
            {
                ReleaseFocus();
                SetState(EState.Idle);
            }
            return;
        }
        if (PlayerTransform == null)
        {
            PlayerTransform = Detecter.DetectedPlayer;
        }
        if (PlayerTransform == null || CurrentState == EState.Stunned || CurrentState == EState.Attacking)
        {
            return;
        }
        if (!TryGetReachablePlayerPosition(out LastReachablePlayerPosition))
        {
            if (HasFocus && (CurrentState == EState.Chasing || CurrentState == EState.Attacking))
            {
                SetState(EState.AttackWaiting);
            }
            return;
        }
        if (!HasFocus)
        {
            AcquireFocus();
            SetState(EState.Chasing);
            return;
        }
        if (CurrentState == EState.AttackWaiting)
        {
            SetState(EState.Chasing);
        }
    }

    private void SetState(EState State)
    {
        if (CurrentState == EState.Death && State != EState.Death)
        {
            return;
        }
        if (CurrentState == State)
        {
            return;
        }
        ExitState(CurrentState);
        CurrentState = State;

        PlaySound();
        switch (CurrentState)
        {
            case EState.Idle:
                StopAgent();
                AnimController.SetBool(IsMovingHash, false);
                IdleTimer = 0.0f;
                IdleInterval = Random.Range(PatrolTimeMin, PatrolTimeMax);
                return;
            case EState.Patrolling:
                Agent.isStopped = false;
                Agent.speed = PatrolSpeed;
                AnimController.SetBool(IsMovingHash, true);
                AnimController.SetFloat(WalkSpeedMultiplierHash, 1.0f);
                SetRandomDestination();
                return;
            case EState.Chasing:
                Agent.isStopped = false;
                Agent.speed = ChaseSpeed;
                AnimController.SetBool(IsMovingHash, true);
                AnimController.SetFloat(WalkSpeedMultiplierHash, PatrolSpeed > 0f ? ChaseSpeed / PatrolSpeed : 1f);
                ChaseToReachablePlayerPosition();
                return;
            case EState.AttackWaiting:
                Agent.isStopped = false;
                Agent.speed = ChaseSpeed;
                AnimController.SetBool(IsMovingHash, true);
                AnimController.SetFloat(WalkSpeedMultiplierHash, PatrolSpeed > 0f ? ChaseSpeed / PatrolSpeed : 1f);
                Agent.SetDestination(LastReachablePlayerPosition);
                return;
            case EState.Attacking:
                StopAgent();
                AnimController.SetBool(IsMovingHash, false);
                AnimController.SetTrigger(DoAttackHash);
                return;
            case EState.Stunned:
                StopAgent();
                AnimController.SetBool(IsMovingHash, false);
                AnimController.SetTrigger(TakeDamageHash);
                return;
            case EState.Death:
                ReleaseFocus();
                StopAgent();
                AnimController.SetBool(IsMovingHash, false);
                AnimController.SetTrigger(DeathTriggerHash);
                return;
            default:
                return;
        }
    }
    private void ExitState(EState State)
    {
        if (State == EState.Attacking)
        {
            EndAttack();
        }
    }
    private void EnterPostAttackState()
    {
        if (Detecter != null && Detecter.IsDetected && TryGetReachablePlayerPosition(out LastReachablePlayerPosition))
        {
            SetState(EState.Chasing);
        }
        else if (Detecter != null && Detecter.IsDetected)
        {
            SetState(EState.AttackWaiting);
        }
        else
        {
            ReleaseFocus();
            SetState(EState.Idle);
        }
    }
    private bool HasArrived()
    {
        return !Agent.pathPending
            && Agent.remainingDistance <= Agent.stoppingDistance
            && Agent.velocity.sqrMagnitude <= 0.01f;
    }
    private void ResumeAfterStun()
    {
        if (CurrentState == EState.Death)
        {
            return;
        }

        if (Detecter != null && Detecter.IsDetected && PlayerTransform != null)
        {
            if (TryGetReachablePlayerPosition(out LastReachablePlayerPosition))
            {
                AcquireFocus();
                SetState(EState.Chasing);
                return;
            }

            if (HasFocus)
            {
                SetState(EState.AttackWaiting);
                return;
            }
        }

        if (IsCombatState(StateBeforeStun))
        {
            SetState(EState.Idle);
        }
        else
        {
            SetState(StateBeforeStun == EState.None ? EState.Idle : StateBeforeStun);
        }
        StateBeforeStun = EState.None;
    }
    private void TryAcquireFocus()
    {
        if (!TryGetReachablePlayerPosition(out LastReachablePlayerPosition))
        {
            return;
        }
        AcquireFocus();
        SetState(EState.Chasing);
    }
    private void AcquireFocus()
    {
        HasFocus = true;
        if (Detecter != null)
        {
            Detecter.SetFocused(true);
        }
    }
    private void ReleaseFocus()
    {
        HasFocus = false;
        EndAttack();
        if (Detecter != null)
        {
            Detecter.SetFocused(false);
        }
    }
    private bool TryGetReachablePlayerPosition(out Vector3 ReachablePosition)
    {
        ReachablePosition = Vector3.zero;
        if (PlayerTransform == null)
        {
            return false;
        }
        if (!NavMesh.SamplePosition(PlayerTransform.position, out NavMeshHit Hit, AttackableNavMeshDistance, NavMesh.AllAreas))
        {
            return false;
        }

        ReachablePosition = Hit.position;
        return true;
    }
    private void ChaseToReachablePlayerPosition()
    {
        if (!TryGetReachablePlayerPosition(out LastReachablePlayerPosition))
        {
            SetState(EState.AttackWaiting);
            return;
        }
        Agent.SetDestination(LastReachablePlayerPosition);
    }
    private void StopAgent()
    {
        if (Agent == null)
        {
            return;
        }
        Agent.velocity = Vector3.zero;
        Agent.isStopped = true;
    }
    private bool IsCombatState(EState State)
    {
        return State == EState.Chasing
            || State == EState.AttackWaiting
            || State == EState.Attacking;
    }
    private void SetRandomDestination()
    {
        Vector3 RandomPos = transform.position + Random.insideUnitSphere * PatrolRadius;
        if (NavMesh.SamplePosition(RandomPos, out NavMeshHit Hit, PatrolRadius, NavMesh.AllAreas))
        {
            Agent.SetDestination(Hit.position);
        }
        else
        {
            Debug.Log("Can't Find Random Destination");
        }
    }
    private void PlaySound()
    {
        if (RuntimeSounds == null || RuntimeSounds.Empty())
        {
            return;
        }
        AudioClip Clip = RuntimeSounds.GetSound();
        if (GatedAudio != null)
        {
            GatedAudio.PlayOneShot(Clip);
            return;
        }

        if (Audio != null)
        {
            Audio.PlayOneShot(Clip);
        }
    }
}
