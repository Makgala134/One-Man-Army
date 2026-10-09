using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy: MonoBehaviour
{
    public Transform player;
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private NavMeshAgent agent;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Close enough: stop and attack
            agent.ResetPath();
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                Debug.Log("Enemy attacks for " + damage); // replace with your damage code
            }
        }
        else if (distance <= detectRange)
        {
            // See the player: chase
            agent.SetDestination(player.position);
        }
        else
        {
            // Too far: stand still
            agent.ResetPath();
        }
    }
}