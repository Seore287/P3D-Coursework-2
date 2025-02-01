using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryUI; 

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
        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);
    }
}
