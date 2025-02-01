using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    [Header("EnemyAI Settings")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float idleDuration = 5f;
    [SerializeField] private int playerDamage = 5;

    [Header("References")]
    public Transform[] patrolPoints;
    public Transform player;
    public NavMeshAgent enemyAgent;
    public Animator animator;

    //private EnemyHealth enemyHealth;
    private int currentPatrolIndex = 0;
    private float idleTimer = 0f;
    private bool isChasing = false;
    private bool isAttacking = false;

    [Header("Gizmos Settings")]
    [SerializeField] private Vector3 sightRangeOffset = Vector3.zero;
    [SerializeField] private Vector3 attackRangeOffset = Vector3.zero;

    private enum AIState { Idle, Patrolling, Chasing, Attacking }
    private AIState currentState = AIState.Idle;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            currentState = AIState.Patrolling;
            enemyAgent.speed = patrolSpeed;
            NextPatrolPoint();
        }
        else
        {
            currentState = AIState.Idle;
        }

        // Ensure NavMeshAgent is properly initialized
        if (enemyAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned!");
        }
    }

    void Update()
    {
        if (enemyAgent == null || !enemyAgent.isOnNavMesh) return;  // Avoid issues if NavMeshAgent is invalid or not on NavMesh

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case AIState.Idle:
                IdleBehavior();
                if (distanceToPlayer <= sightRange)
                {
                    currentState = AIState.Chasing;
                    enemyAgent.speed = chaseSpeed;
                    isChasing = true;
                }
                break;

            case AIState.Patrolling:
                PatrolBehavior();
                if (distanceToPlayer <= sightRange)
                {
                    currentState = AIState.Chasing;
                    enemyAgent.speed = chaseSpeed;
                    isChasing = true;
                }
                break;

            case AIState.Chasing:
                ChaseBehavior();
                if (distanceToPlayer <= attackRange)
                {
                    currentState = AIState.Attacking;
                    animator.SetBool("IsRunning", false);
                    animator.SetBool("IsAttacking", true);
                }
                else if (distanceToPlayer > sightRange)
                {
                    currentState = AIState.Patrolling;
                    isChasing = false;
                    enemyAgent.speed = patrolSpeed;
                    NextPatrolPoint();
                }
                break;

            case AIState.Attacking:
                AttackBehavior();
                if (distanceToPlayer > attackRange)
                {
                    currentState = AIState.Chasing;
                    isChasing = true;
                    animator.SetBool("IsAttacking", false);
                }
                break;
        }
        RotateTowards();
    }

    private void IdleBehavior()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            idleTimer = 0;
            currentState = AIState.Patrolling;
            NextPatrolPoint();
        }

        EnemyLook();
        animator.SetBool("IsWalking", false);
    }

    private void PatrolBehavior()
    {
        if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.5f)
        {
            NextPatrolPoint();
        }
        animator.SetBool("IsWalking", true);
    }

    private void ChaseBehavior()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsWalking", false);
        enemyAgent.SetDestination(player.position);
    }

    private void AttackBehavior()
{
    if (!isAttacking)
    {
        isAttacking = true;
        enemyAgent.isStopped = true;
        enemyAgent.SetDestination(transform.position);  // Stop moving while attacking

        int attackType = Random.Range(0, 2);
        animator.SetFloat("AttackType", attackType);
        animator.SetTrigger("Attack");

        StartCoroutine(PerformAttack());
    }
}

private IEnumerator PerformAttack()
{
    yield return new WaitForSeconds(0.5f); // Adjust timing to match attack animation
    if (Vector3.Distance(transform.position, player.position) <= attackRange)
    {
        if (player.TryGetComponent<PlayerStats>(out var playerStats))
        {
            Debug.Log("Enemy hit the player!");
            playerStats.TakeDamage(playerDamage);
        }
    }
    yield return new WaitForSeconds(0.5f); // Give some delay before attacking again
    isAttacking = false;
    enemyAgent.isStopped = false;
}




    private void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // Only set destination if NavMeshAgent is valid
        if (enemyAgent != null && enemyAgent.isOnNavMesh)
        {
            enemyAgent.destination = patrolPoints[currentPatrolIndex].position;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void EnemyLook()
    {
        float randomAngle = Random.Range(-90, 90);
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
    }

    private void RotateTowards()
    {
        if (enemyAgent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(enemyAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemyAgent.angularSpeed);
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        enemyAgent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + sightRangeOffset, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackRangeOffset, attackRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (patrolPoints != null)
        {
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.1f);
                }
            }
        }
    }
}
