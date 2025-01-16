using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxStamina = 50;
    [SerializeField] private int maxXP = 100; // Maximum XP for current level
    [SerializeField] private float healthRecoveryRate = 1f;
    [SerializeField] private float staminaRecoveryRate = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject loadingScreen;

    private int currentHealth;
    private int currentStamina;
    private int currentXP = 0; // Player's current XP

    private Animator animator;

    // Flag to track if the death animation has been triggered
    private bool hasDied = false;

    // Public properties to expose health, stamina, and XP values
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float Stamina => currentStamina;
    public float MaxStamina => maxStamina;
    public int CurrentXP => currentXP;
    public int MaxXP => maxXP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        if (gameOver != null) gameOver.SetActive(false);
        if (loadingScreen != null) loadingScreen.SetActive(false);
    }

    private void Update()
    {
        // Regenerate health over time
        if (currentHealth < maxHealth && !hasDied)
        {
            currentHealth = Mathf.Min(currentHealth + Mathf.RoundToInt(healthRecoveryRate * Time.deltaTime), maxHealth);
        }

        // Regenerate stamina over time
        if (currentStamina < maxStamina && !hasDied)
        {
            currentStamina = Mathf.Min(currentStamina + Mathf.RoundToInt(staminaRecoveryRate * Time.deltaTime), maxStamina);
        }

        if (currentHealth <= 0 && !hasDied)
        {
            Die();
        }
    }

    /// <summary>
    /// Adds XP to the player's current XP.
    /// </summary>
    /// <param name="xp">Amount of XP to add.</param>
    public void AddXP(int xp)
    {
        currentXP += xp;

        // Check if XP exceeds the current level's max XP
        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Handles player leveling up and resets XP for the new level.
    /// </summary>
    private void LevelUp()
    {
        currentXP -= maxXP; // Carry over excess XP to the next level
        maxXP += 50;        // Example: Increase max XP for each level
        Debug.Log("Level Up! New Max XP: " + maxXP);
        // Add level-up rewards or effects here
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void RegenerateStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
    }

    private void Die()
    {
        if (!hasDied)
        {
            hasDied = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        if (gameOver != null) gameOver.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (gameOver != null) gameOver.SetActive(false);
        if (loadingScreen != null) loadingScreen.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(2);
    }
}
