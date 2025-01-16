using UnityEngine;

[CreateAssetMenu(fileName = "Axe Smash", menuName = "Abilities/Axe Smash")]
public class AxeAbility : Ability
{
    public float areaOfEffect = 5f;
    public float damage = 30f;

    public override void Activate(GameObject player)
    {
        // Add axe-specific ability logic here
        Debug.Log($"Axe Smash activated! Deals {damage} damage in a {areaOfEffect}m radius.");
        // Optionally, add area-of-effect logic or animations
    }
}
