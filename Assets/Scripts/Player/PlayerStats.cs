using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxStamina = 50;
    [SerializeField] private int maxXP = 100; 
    [SerializeField] private float healthRecoveryRate = 1f;
    [SerializeField] private float staminaRecoveryRate = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text xpText;

    private int currentHealth;
    private int currentStamina;
    private int currentXP = 0; 

    private Animator animator;


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

        UpdateXPText();

        if (currentHealth <= 0 && !hasDied)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void RegenerateStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
    }

    public void AddXP(int xp)
    {
        currentXP += xp;

        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= maxXP; 
        maxXP += 50;        
    }

    private void UpdateXPText() 
    {
        if (xpText != null) 
        {
            xpText.text = "XP: " + currentXP;
        }
    }

    public void TakeDamage(int damage)
    {
        if (animator != null) 
        {
            animator.SetTrigger("Hit");
            StartCoroutine(ResetAnimation());
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
    }

    private IEnumerator ResetAnimation() 
    {
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("Hit");
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
