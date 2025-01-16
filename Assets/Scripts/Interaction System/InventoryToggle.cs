using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryUI;  // Reference to the inventory UI panel

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.OpenInventory.performed += _ => ToggleInventory();
        playerInput.Gameplay.OpenInventory.performed -= _ => ToggleInventory();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable(); 
    }

    public void ToggleInventory()
    {
        // Toggle the inventory panel active state
        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);
    }
}
