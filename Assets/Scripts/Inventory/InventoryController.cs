using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    public GameObject inventoryUI;

    private PlayerInput playerInput;
    private bool isInventoryOpen = false;

    private int selectedSlotIndex = 0;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.OpenInventory.performed += _ => OnOpenInventory();
        playerInput.Gameplay.UseItem.performed += _ => OnUseInventoryItem();
    }

    private void OnEnable()
    {
        playerInput.Enable();   
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    public void OnOpenInventory()
    {
        ToggleInventory();
    }

    private void ToggleInventory()
    {
        if (inventoryUI == null)
        {
            Debug.LogError("Inventory UI is not assigned!");
            return;
        }

        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
    }

    private void OnUseInventoryItem()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found!");
            return;
        }

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is not assigned!");
            return;
        }

        if (selectedSlotIndex < 0 || selectedSlotIndex >= inventoryManager.slots.Count)
        {
            Debug.LogWarning("Invalid inventory slot selected!");
            return;
        }

        // Check if the selected slot contains a usable item
        var selectedSlot = inventoryManager.slots[selectedSlotIndex];

        if (selectedSlot.IsEmpty())
        {
            Debug.LogWarning("Selected slot is empty!");
            return;
        }

        if (!selectedSlot.item.isUsable)
        {
            Debug.LogWarning($"Item '{selectedSlot.item.itemName}' is not usable!");
            return;
        }

        // Use the item
        inventoryManager.UseItem(selectedSlotIndex, playerStats);
    }

    public void UseInventoryItem(int slotIndex)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            inventoryManager.UseItem(slotIndex, playerStats);
        }
        else
        {
            Debug.LogError("PlayerStats not found!");
        }
    }

    public void SetSelectedSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryManager.slots.Count)
        {
            Debug.LogError("Invalid slot index!");
            return;
        }

        selectedSlotIndex = slotIndex;
    }

    public void OnSlotClicked(int slotIndex)
    {
        UseInventoryItem(slotIndex);
    }

}
