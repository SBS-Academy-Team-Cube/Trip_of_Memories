using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EState { Idle, Patrolling, Chasing, Attacking, Stunned, Death, None };

public class EnemyAI : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    public EState CurrentState;
    private EState PreviousState;

    public float PatrolSpeed, ChaseSpeed;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator AnimController;
    [SerializeField] private EnemyDetecter Detecter;
    [SerializeField] private EnemyAttacker Attacker;
    [SerializeField] private Health HP;


    [SerializeField] private float PatrolRadius = 5f;
    [SerializeField] private float PatrolTimeMin = 1.5f;
    [SerializeField] private float PatrolTimeMax = 5f;

    
    private Transform PlayerTransform = null;

    private float IdleInterval;
    private float IdleTimer = 0.0f;

    private bool IsPatrolling = false;
    void Awake()
    {
        PatrolRadius *= transform.lossyScale.x;
    }
    
    private void OnEnable() 
    {
        if(Detecter != null)
        {
            Detecter.OnPlayerDetected += OnPlayerFound;
        }
        if(HP != null)
        {
            HP.OnHPChanged += OnTakeDamage;
            HP.OnDead += OnDead;
        }
    }
    private void OnDead()
    {
        SetState(EState.Death);
    }
    private void OnTakeDamage(int HP)
    {
        PreviousState = CurrentState;
        SetState(EState.Stunned);
    }
    
    private void OnDisable() {
        if(Detecter != null)
        {
            Detecter.OnPlayerDetected -= OnPlayerFound;
        }
        if(HP != null)
        {
            HP.OnHPChanged -= OnTakeDamage;
            HP.OnDead -= OnDead;
        }
    }
    private void StartAttack()
    {
        if(Attacker != null)
        {
            Attacker.IsAttacking = true;
        }
    }
    private void EndAttack()
    {
        if(Attacker != null)
        {
            Attacker.IsAttacking = false;
        }
    }
    private void OnPlayerFound(bool bFound, Transform TargetPlayer)
    {
        if(bFound)
        {
            PlayerTransform = TargetPlayer;
            TryChaseToPlayer();
        }
        else
        {
            PlayerTransform = TargetPlayer;
            SetState(EState.Idle);
        }
    }
    void Start()
    {
        SetRandomDestination();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PatrolRadius);
    }
    void Update()
    {
        if(Detecter.IsDetected && CurrentState != EState.Chasing && CurrentState != EState.Attacking)
        {
            TryChaseToPlayer();
        }
        if(CurrentState == EState.Death)
        {
            return;
        }
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
                if(Attacker.CanAttack())
                {
                    SetState(EState.Attacking);
                }
                else
                {
                    if(HasArrived())
                    {
                        LookAtPlayer();
                    }
                    else
                    {
                        TryChaseToPlayer();
                    }
                }
                return;
            default:
                break;
        }
    }

    private void LookAtPlayer()
    {
        if(PlayerTransform == null)
        {
            return;
        }

        Vector3 Direction = PlayerTransform.position - transform.position;
        Direction.y = 0f;

        if(Direction.sqrMagnitude <= 0.001f)
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
    private void SetState(EState State)
    {
        CurrentState = State;
        switch(CurrentState)
        {
            case EState.Idle:
                AnimController.SetBool(IsMovingHash, false);
                IdleTimer = 0.0f;
                IdleInterval = Random.Range(PatrolTimeMin, PatrolTimeMax);
                return;
            case EState.Patrolling:
                Agent.isStopped = false;
                Agent.speed = PatrolSpeed;
                AnimController.SetBool("IsMoving", true);
                AnimController.SetFloat("WalkSpeedMultiplier", 1.0f);
                SetRandomDestination();
                return;
            case EState.Chasing:
                Agent.isStopped = false;
                Agent.speed = ChaseSpeed;
                AnimController.SetBool("IsMoving", true);
                AnimController.SetFloat("WalkSpeedMultiplier", ChaseSpeed / PatrolSpeed);
                return;
            case EState.Attacking:
                Agent.velocity = Vector3.zero;
                Agent.isStopped = true;
                AnimController.SetBool("IsMoving", false);
                AnimController.SetTrigger("DoAttack");
                StartCoroutine(AttackRoutine());
                return;
            case EState.Stunned:
                Agent.velocity = Vector3.zero;
                Agent.isStopped = true;
                AnimController.SetTrigger("TakeDamage");
                StartCoroutine(StunnedRoutine());
                return;
            case EState.Death:
                Agent.velocity = Vector3.zero;
                Agent.isStopped = true;
                AnimController.SetTrigger("DeathTrigger");
                StartCoroutine(DeathRoutine());
                return;
            default:
                return;
        }
    }
    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(AnimController.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(gameObject);
    }
    private IEnumerator StunnedRoutine()
    {
        yield return new WaitForSeconds(AnimController.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.1f);
        SetState(PreviousState);
        PreviousState = EState.None;
    }
    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(AnimController.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.1f);
        SetState(EState.Chasing);
    }
    private bool HasArrived()
    {
        return Agent.remainingDistance <= Agent.stoppingDistance && Agent.velocity.sqrMagnitude <= 0.01f;
    }
    private void TryChaseToPlayer()
    {
        if(PlayerTransform == null)
        {
            return;
        }
        if(NavMesh.SamplePosition(PlayerTransform.position, out NavMeshHit Hit, 0.85f, NavMesh.AllAreas))
        {
            Debug.Log("Can Chase Player");
            SetState(EState.Chasing);
            Agent.SetDestination(Hit.position);
        }
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
}