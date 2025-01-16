using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 20;

    private void Start()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.item == item && item.isStackable)
            {
                slot.AddItem(item, amount);
                return true;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item, amount);
                return true;
            }
        }

        Debug.Log("Inventory Full!");
        return false;
    }

    public void UseItem(int slotIndex, PlayerStats playerStats)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count && !slots[slotIndex].IsEmpty())
        {
            slots[slotIndex].item.Use(playerStats);

            if (slots[slotIndex].item.isStackable)
            {
                slots[slotIndex].RemoveItem(1);
            }
            else
            {
                slots[slotIndex].RemoveItem(slots[slotIndex].quantity);
            }
        }
    }
}
