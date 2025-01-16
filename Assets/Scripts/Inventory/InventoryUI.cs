using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    public Transform slotContainer;          
    public GameObject slotPrefab;            

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Clear existing slots
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate new slots
        foreach (InventorySlot slot in inventoryManager.slots)
        {
            GameObject slotInstance = Instantiate(slotPrefab, slotContainer);
            UpdateSlotUI(slotInstance, slot);
        }
    }

    private void UpdateSlotUI(GameObject slotInstance, InventorySlot slot)
    {
        // Get references to the slot's icon and quantity text
        Image icon = slotInstance.transform.GetChild(1).GetComponent<Image>();
        TMP_Text quantityText = slotInstance.transform.GetChild(2).GetComponent<TMP_Text>();

        // Debugging: Check if we successfully retrieved the components
        if (icon == null)
        {
            Debug.LogError("Image component not found in slot prefab.");
        }
        if (quantityText == null)
        {
            Debug.LogError("Text component not found in slot prefab.");
        }

        // Update the slot UI based on whether the slot is empty or not
        if (slot.IsEmpty())
        {
            icon.enabled = false;
            quantityText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            quantityText.text = slot.item.isStackable ? slot.quantity.ToString() : "";
        }
    }
}
