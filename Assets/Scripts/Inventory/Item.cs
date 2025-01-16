using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStack;
    public bool isStackable;

    public enum ItemType { Heal, Stamina, Buy, Equip, Read, See }
    public ItemType type;
    public bool isUsable;
    public int itemEffectValue;


    public void Use(PlayerStats playerStats)
    {
        switch (type)
        {
            case ItemType.Heal:
                playerStats.Heal(itemEffectValue); 
                break;
             case ItemType.Stamina:
                playerStats.RegenerateStamina(itemEffectValue); 
                break;
        }
    }
}
