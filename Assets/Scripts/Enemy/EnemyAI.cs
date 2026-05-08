using UnityEngine;
using UnityEngine.AI;

public enum EState { Idle, Patrolling, Chasing, Attacking, Stunned };

public class EnemyAI : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    public EState CurrentState;
    
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator AnimController;
    [SerializeField] private EnemyDetecter Detecter;
    [SerializeField] private EnemyAttacker Attacker;


    

    [SerializeField] private float PatrolRadius = 5f;
    [SerializeField] private float PatrolTimeMin = 1.5f;
    [SerializeField] private float PatrolTimeMax = 5f;

    
    private Transform PlayerTransform = null;

    private float PatrolInterval;
    private float PatrolTimer = 0.0f;

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
    }
    private void OnDisable() {
        if(Detecter != null)
        {
            Detecter.OnPlayerDetected -= OnPlayerFound;
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
            CurrentState = EState.Chasing;
            PlayerTransform = TargetPlayer;

            ChaseToPlayer();
        }
        else
        {

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
        switch (CurrentState)
        {
            case EState.Idle:
                PatrolTimer += Time.deltaTime;
                if (PatrolTimer >= PatrolInterval)
                {
                    PatrolTimer = 0.0f;
                    SetRandomDestination();
                }
                return;

            case EState.Patrolling:
                if (HasArrived())
                {
                    PatrolInterval = Random.Range(PatrolTimeMin, PatrolTimeMax);
                    AnimController.SetBool(IsMovingHash, false);
                    CurrentState = EState.Idle;
                }
                return;

            case EState.Chasing:
                if(HasArrived())
                {
                    Debug.Log("Enemy Arrived to Player");

                    if(Attacker.CanAttack())
                    {
                        AnimController.SetBool("IsMoving", false);
                        AnimController.SetTrigger("DoAttack");
                    }
                    else
                    {
                        ChaseToPlayer();
                    }
                }
                else
                {
                    ChaseToPlayer();
                }
                return;
            default:
                break;
        }


        
        // if (CurrentState == EState.Idle)
        // {
        //     PatrolTimer += Time.deltaTime;
        //     if (PatrolTimer >= PatrolInterval)
        //     {
        //         PatrolTimer = 0.0f;
        //         SetRandomDestination();
        //     }
        //     return;
        // }
        // if (CurrentState == EState.Patrolling)
        // {
        //     if (HasArrived())
        //     {
        //         PatrolInterval = Random.Range(PatrolTimeMin, PatrolTimeMax);
        //         AnimController.SetBool(IsMovingHash, false);
        //         CurrentState = EState.Idle;
        //     }
        //     return;
        // }
    }

    private bool HasArrived()
    {
        return Agent.remainingDistance <= Agent.stoppingDistance && Agent.velocity.sqrMagnitude <= 0.01f;
    }

    private void ChaseToPlayer()
    {
        if(NavMesh.SamplePosition(PlayerTransform.position, out NavMeshHit Hit, 2.5f, NavMesh.AllAreas))
        {
            AnimController.SetBool("IsMoving", true);
            AnimController.SetFloat("WalkSpeedMultiplier", 1.5f);
            Agent.SetDestination(Hit.position);
        }
    }

    private void SetRandomDestination()
    {
        Vector3 RandomPos = transform.position + Random.insideUnitSphere * PatrolRadius;
        if (NavMesh.SamplePosition(RandomPos, out NavMeshHit Hit, PatrolRadius, NavMesh.AllAreas))
        {
            CurrentState = EState.Patrolling;
            AnimController.SetBool("IsMoving", true);
            AnimController.SetFloat("WalkSpeedMultiplier", 1.0f);
            Agent.SetDestination(Hit.position);
        }
    }
}