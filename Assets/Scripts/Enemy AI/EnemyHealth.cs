using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMP_Text healthTextPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);

    private TMP_Text floatingTextInstance;

    public Vector3 randomIntensity = new Vector3(0.5f, 0, 0);

    private int currentHealth;

    private EnemyAIBase enemy;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthTextPrefab != null)
        {
            healthTextPrefab.gameObject.SetActive(false); 
        }

        enemy = GetComponent<EnemyAIBase>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking damage: {damage}");
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        ShowDamage();

        if (enemy.animator != null) 
        {
            enemy.animator.SetTrigger("EnemyHit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<PlayerCombat>(out var playerCombat))
            {
                TakeDamage((int)playerCombat.LightAttackDamage);
            }
        }
    }

    private void ShowDamage()
    {
        if (healthTextPrefab != null)
        {
            if (floatingTextInstance == null)
            {
                // If the floating text doesn't exist, instantiate it
                Vector3 randomOffset = new Vector3(
                    Random.Range(-randomIntensity.x, randomIntensity.x),
                    Random.Range(-randomIntensity.y, randomIntensity.y),
                    Random.Range(-randomIntensity.z, randomIntensity.z));

                Vector3 spawnPosition = transform.position + offset + randomOffset;
                floatingTextInstance = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, transform);
            }

            floatingTextInstance.gameObject.SetActive(true);

            floatingTextInstance.text = $"{currentHealth}";

            
            floatingTextInstance.transform.SetPositionAndRotation(transform.position + offset + new Vector3(
                Random.Range(-randomIntensity.x, randomIntensity.x),
                Random.Range(-randomIntensity.y, randomIntensity.y),
                Random.Range(-randomIntensity.z, randomIntensity.z)), Quaternion.Euler(0f, 90f, 0f));
            Destroy(floatingTextInstance.gameObject, 2f);
        }
    }

    private void Die()
    {
        enemy.enemyAgent.isStopped = true;
        enemy.animator.SetTrigger("Die");
        Destroy(gameObject, 2f); 
    }
}
