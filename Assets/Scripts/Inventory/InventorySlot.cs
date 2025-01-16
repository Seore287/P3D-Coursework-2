using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public bool IsEmpty() => item == null;

    public void AddItem(Item newItem, int amount = 1)
    {
        if (item == null)
        {
            item = newItem;
            quantity = amount;
        }
        else if (item == newItem && item.isStackable)
        {
            quantity = Mathf.Min(quantity + amount, item.maxStack);
        }
    }

    public void RemoveItem(int amount = 1)
    {
        if (item == null) return;

        quantity -= amount;
        if (quantity <= 0)
        {
            item = null;
            quantity = 0;
        }
    }
}
