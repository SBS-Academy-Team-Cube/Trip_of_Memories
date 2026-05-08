using UnityEngine;
using UnityEngine.AI;

public enum EState { Idle, Patrolling, Chasing, Attacking, Stunned };
public class EnemyAI : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private EState CurrentState;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Animator AnimController;
    [SerializeField] private float PatrolRadius = 5f;
    [SerializeField] private float PatrolTimeMin = 1.5f;
    [SerializeField] private float PatrolTimeMax = 5f;


    private float PatrolInterval;
    private float PatrolTimer = 0.0f;

    private bool IsPatrolling = false;
    void Awake()
    {
        PatrolRadius *= transform.lossyScale.x;
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
        if (CurrentState == EState.Idle)
        {
            PatrolTimer += Time.deltaTime;
            if (PatrolTimer >= PatrolInterval)
            {
                PatrolTimer = 0.0f;
                SetRandomDestination();
            }
            return;
        }
        if (CurrentState == EState.Patrolling)
        {
            if (HasArrived())
            {
                PatrolInterval = Random.Range(PatrolTimeMin, PatrolTimeMax);
                AnimController.SetBool(IsMovingHash, false);
                CurrentState = EState.Idle;
            }
            return;
        }
    }

    private bool HasArrived()
    {
        return Agent.remainingDistance <= Agent.stoppingDistance && Agent.velocity.sqrMagnitude <= 0.01f;
    }
    void SetRandomDestination()
    {
        Vector3 RandomPos = transform.position + Random.insideUnitSphere * PatrolRadius;
        if (NavMesh.SamplePosition(RandomPos, out NavMeshHit hit, PatrolRadius, NavMesh.AllAreas))
        {
            CurrentState = EState.Patrolling;
            AnimController.SetBool("IsMoving", true);
            Agent.SetDestination(hit.position);
        }
    }
}