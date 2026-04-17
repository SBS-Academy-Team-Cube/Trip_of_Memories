using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator AnimController;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float patrolInterval = 3f;

    private float timer;

    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= patrolInterval)
        {
            timer = 0f;
            SetRandomDestination();
        }
    }
    void SetRandomDestination()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * patrolRadius;
    
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}