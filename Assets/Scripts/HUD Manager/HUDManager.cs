using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public PlayerStats playerStats;

    public Slider healthBar;
    public Slider staminaBar;

    private void Update()
    {
        if (playerStats != null)
        {
            UpdateHealthBar();
            UpdateStaminaBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.CurrentHealth / playerStats.MaxHealth;
        }
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = playerStats.Stamina / playerStats.MaxStamina;
        }
    }
}
