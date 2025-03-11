using UnityEngine;
using UnityEngine.AI; // for navmesh
using System.Threading.Tasks; // so that we dont need to use coroutines to delay code

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform pointA, pointB, playerPosition;
    public Transform currentTarget;
    public float chaseRadius;
    public bool isChasing = false;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.speed = 0;    
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer =
            Vector3.Distance(transform.position, playerPosition.position);
        if (distanceToPlayer <= chaseRadius)
        {
            isChasing = true;
            agent.speed = 5;
            //currentTarget = playerPosition;
            agent.SetDestination(playerPosition.position);
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                agent.SetDestination(currentTarget.position);
            }
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // switch target
                currentTarget = (currentTarget == pointA) ? pointB : pointA;
                agent.SetDestination(currentTarget.position);
            }
        }
            // pathpending (boolean, checking if there's a path or not)
            // remaining distance (float)
            // velocity = 0 (minsan di 'to applicable i.e pag naka-idle si enemy)
        }

    public async void StartPatrolling()
    {
        await Task.Delay(5000);
        agent.speed = 3.5f;
        currentTarget = pointA;
        // making the agent move to Point A muna
        agent.SetDestination(currentTarget.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
