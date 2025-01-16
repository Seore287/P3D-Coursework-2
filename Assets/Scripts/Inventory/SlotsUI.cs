using UnityEngine;
using UnityEngine.UI;

public class SlotsUI : MonoBehaviour
{
    public Image icon;
    public Text quantityText;

    public void SetSlot(InventorySlot slot)
    {
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
