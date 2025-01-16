using UnityEngine;
using System.Collections.Generic;

public class CooldownManager : MonoBehaviour
{
    [Header("Cooldown Settings")]
    public float staminaCooldownTime = 2f;

    private Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();
    private Dictionary<string, float> abilityTimers = new Dictionary<string, float>();
    private float staminaTimer;

    private void Update()
    {
        HandleCooldowns();
    }

    private void HandleCooldowns()
    {
        List<string> keys = new List<string>(abilityTimers.Keys);

        foreach (string key in keys)
        {
            abilityTimers[key] -= Time.deltaTime;
            if (abilityTimers[key] <= 0)
            {
                abilityTimers.Remove(key);
                Debug.Log($"{key} cooldown finished.");
            }
        }

        if (staminaTimer > 0)
        {
            staminaTimer -= Time.deltaTime;
        }
    }

    public void StartAbilityCooldown(string abilityName, float cooldownTime)
    {
        if (!abilityCooldowns.ContainsKey(abilityName))
        {
            abilityCooldowns.Add(abilityName, cooldownTime);
        }

        if (!abilityTimers.ContainsKey(abilityName))
        {
            abilityTimers.Add(abilityName, cooldownTime);
            Debug.Log($"{abilityName} is on cooldown for {cooldownTime} seconds.");
        }
    }

    public bool IsAbilityOnCooldown(string abilityName)
    {
        return abilityTimers.ContainsKey(abilityName);
    }

    public void StartStaminaCooldown()
    {
        staminaTimer = staminaCooldownTime;
        Debug.Log("Stamina is on cooldown for " + staminaCooldownTime + " seconds.");
    }

    public bool IsStaminaOnCooldown()
    {
        return staminaTimer > 0;
    }
}
