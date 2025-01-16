using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyAI = other.GetComponent<EnemyHealth>();
            if (enemyAI != null && playerCombat != null)
            {
                AnimatorStateInfo stateInfo = playerCombat.Animator.GetCurrentAnimatorStateInfo(0);
                float damage = stateInfo.IsName("HeavyAttack") ? playerCombat.HeavyAttackDamage
                    : playerCombat.LightAttackDamage;

                enemyAI.TakeDamage((int)damage);
            }
        }
    }
}
