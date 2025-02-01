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
        if (enemyAgent == null || !enemyAgent.isOnNavMesh) return;  

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
        
        animator.SetBool("IsRunning", false); 
        animator.SetBool("IsAttacking", false); 
        animator.SetBool("IsWalking", true);
    }

    private void ChaseBehavior()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
    
        enemyAgent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > sightRange)
        {
            animator.SetBool("IsAttacking", false); 
            animator.SetBool("IsRunning", false); 
            
            // Transition back to patrolling
            isChasing = false;
            currentState = AIState.Patrolling;
            enemyAgent.speed = patrolSpeed;

            NextPatrolPoint();
        }
    }

    private void AttackBehavior()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            enemyAgent.isStopped = true; // Stop enemy movement
            animator.SetTrigger("Attack");

            StartCoroutine(PerformAttack());
        }
        
        // If player moves out of attack range, return to chase or patrol
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            isAttacking = false;
            enemyAgent.isStopped = false;
            
            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                Debug.Log("Player left attack and sight range. Returning to patrol.");
                currentState = AIState.Patrolling;
                enemyAgent.speed = patrolSpeed;
                NextPatrolPoint();
            }
            else
            {
                Debug.Log("Player out of attack range. Resuming chase.");
                currentState = AIState.Chasing;
                enemyAgent.speed = chaseSpeed;
            }
        }
    }
    
    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(0.25f); 
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.TryGetComponent<PlayerStats>(out var playerStats))
            {
                Debug.Log("Enemy hit the player!");
                playerStats.TakeDamage(playerDamage);
            }
        }
        
        // Wait for animation time before resetting attack state
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 

        // Reset attack to the related states after attack is completed
        isAttacking = false;
        animator.SetBool("IsAttacking", false); 

        enemyAgent.isStopped = false;
    } 

    private void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

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
