using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CooldownManager cooldownManager;
    [SerializeField] private AbilityManager abilityManager;

    [Header("Combat References")]
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private GameObject swordWeapon;
    [SerializeField] private GameObject axeWeapon;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private Collider axeCollider;

    [Header("Combat Settings")]
    private float lightAttackDamage = 5f;
    private float heavyAttackDamage = 10f;
    private float attackCooldown = 0.5f;

    [Header("Shield Settings")]
    [SerializeField] private GameObject shieldParticleSystem; 
    private Collider shieldCollider; 
    private bool isShieldActive = false;

    private bool isAttacking = false;
    private bool isEquipped = false;
    private bool isSwordActive = true;

    public Animator Animator { get => animator; set => animator = value; }
    public Transform RightHandTransform { get => rightHandTransform; }
    public Transform RightTransform { get => rightTransform; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float HeavyAttackDamage { get => heavyAttackDamage; set => heavyAttackDamage = value; }
    public float LightAttackDamage { get => lightAttackDamage; set => lightAttackDamage = value; }
    public Collider ShieldCollider { get; internal set; }

    public enum WeaponType { sword, axe }

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.LightAttack.performed += _ => PerformLightAttack();
        playerInput.Gameplay.HeavyAttack.performed += _ => PerformHeavyAttack();
        playerInput.Gameplay.Block.performed += _ => ToggleShield();
        playerInput.Gameplay.Equip.performed += _ => ToggleWeapon();
        playerInput.Gameplay.UseAbility1.performed += _ => UseAbility(0);
        playerInput.Gameplay.UseAbility2.performed += _ => UseAbility(1);

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        swordCollider.enabled = false;
        axeCollider.enabled = false;

        if (shieldParticleSystem != null)
        {
            shieldCollider = shieldParticleSystem.GetComponent<Collider>();
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Start()
    {
        swordCollider.enabled = false;
        axeCollider.enabled = false;
        swordWeapon.SetActive(true);
        axeWeapon.SetActive(false);

        DeactivateShield();
    }
   
    private void UseAbility(int index)
    {
        if (abilityManager == null)
        {
            Debug.LogWarning("AbilityManager is not assigned.");
            return;
        }

        // Ensure the ability index is valid
        if (index >= 0 && index < abilityManager.abilities.Count)
        {
            Ability ability = abilityManager.abilities[index];
            if (cooldownManager != null && cooldownManager.IsAbilityOnCooldown(ability.abilityName))
            {
                Debug.Log($"{ability.abilityName} is on cooldown.");
                return;
            }

            abilityManager.ActivateAbility(index);

            if (cooldownManager != null)
            {
                cooldownManager.StartAbilityCooldown(ability.abilityName, ability.cooldownTime);
            }
        }
        else
        {
            Debug.LogWarning("Invalid ability index.");
        }
    }

    public void PerformLightAttack()
    {
        if (isAttacking || !isEquipped) return;
        isAttacking = true;

        if (isSwordActive)
        {
            animator.SetTrigger("LightAttack");
            swordCollider.enabled = true;
        }
        else
        {
            animator.SetTrigger("AxeAttack1");
            axeCollider.enabled = true;
        }

        StartCoroutine(AttackCooldownRoutine());
    }

    public void PerformHeavyAttack()
    {
        if (isAttacking || !isEquipped) return;
        isAttacking = true;

        if (isSwordActive)
        {
            animator.SetTrigger("HeavyAttack");
            swordCollider.enabled = true;
        }
        else
        {
            animator.SetTrigger("AxeAttack2");
            axeCollider.enabled = true;
        }

        StartCoroutine(AttackCooldownRoutine());
    }

    public void ActivateShield()
    {
        isShieldActive = true;

        if (shieldParticleSystem != null)
            shieldParticleSystem.SetActive(true);

        if (shieldCollider != null)
            shieldCollider.enabled = true;
    }

    public void DeactivateShield()
    {
        isShieldActive = false;

        if (shieldParticleSystem != null)
            shieldParticleSystem.SetActive(false);

        if (shieldCollider != null)
            shieldCollider.enabled = false;
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(AttackCooldown);

        // Disable the correct collider after the attack
        if (isSwordActive)
        {
            swordCollider.enabled = false;
        }
        else
        {
            axeCollider.enabled = false;
        }

        isAttacking = false;
    }

    private void ToggleWeapon()
    {
        if (isSwordActive)
        {
            EquipAxeWeapon();
        }
        else
        {
            EquipSword();
        }
    }

    private void EquipSword()
    {
        isSwordActive = true;
        isEquipped = true;
        animator.SetTrigger("Equip");

        swordWeapon.SetActive(true);
        axeWeapon.SetActive(false); 

        swordWeapon.transform.SetParent(rightHandTransform);
        swordWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        abilityManager.SetWeaponState(WeaponType.sword);
    }

    private void EquipAxeWeapon()
    {
        isSwordActive = false;
        isEquipped = true;
        animator.SetTrigger("Equip");

        axeWeapon.SetActive(true);
        swordWeapon.SetActive(false); 

        axeWeapon.transform.SetParent(rightTransform);
        axeWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        abilityManager.SetWeaponState(WeaponType.axe);
    }

    private void ToggleShield()
    {
        if (isShieldActive)
        {
            DeactivateShield();
        }
        else
        {
            ActivateShield();
        }
    }
}
