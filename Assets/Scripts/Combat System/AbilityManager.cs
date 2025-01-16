using UnityEngine;
using System.Collections.Generic;
using static PlayerCombat;

public class AbilityManager : MonoBehaviour
{
    [Header("Ability Settings")]
    public List<Ability> abilities = new List<Ability>();

    [Header("Player References")]
    private PlayerInput input;
    private GameObject player;
    private CooldownManager cooldownManager;
    private PlayerCombat playerCombat;

    private WeaponType currentWeapon = WeaponType.sword;

    private void Awake()
    {
        input = new PlayerInput();
        input.Gameplay.UseAbility1.performed += _ => ActivateAbility(0);
        input.Gameplay.UseAbility2.performed += _ => ActivateAbility(1);

        player = gameObject; 

        playerCombat = GetComponent<PlayerCombat>();
        cooldownManager = GetComponent<CooldownManager>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void ActivateAbility(int index)
    {
        if (index < 0 || index >= abilities.Count)
        {
            Debug.LogWarning("Invalid ability index.");
            return;
        }

        if (cooldownManager == null)
        {
            Debug.LogError("CooldownManager is not assigned or found.");
            return;
        }

        Ability ability = abilities[index];

        
        if (currentWeapon == WeaponType.sword && !ability.CanBeUsedWithSword)
        {
            return;
        }
        else if (currentWeapon == WeaponType.axe && !ability.CanBeUsedWithAxe)
        {
            return;
        }

        // Checks if the ability is on cooldown
        if (cooldownManager.IsAbilityOnCooldown(ability.abilityName))
        {
            return;
        }


        ability.Activate(player);

        // Begins the ability cooldown based on the ability
        cooldownManager.StartAbilityCooldown(ability.abilityName, ability.cooldownTime);
    }

    public void SetWeaponState(WeaponType weaponType)
    {
        currentWeapon = weaponType;
    }
}
