using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;               
    public int quantity = 1;        

    private InventoryManager inventoryManager;  
    private InventoryUI inventoryUI;            

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool itemAdded = inventoryManager.AddItem(item, quantity);

            if (itemAdded)
            {
                inventoryUI.RefreshUI();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full, cannot pick up item.");
            }
        }
    }
}
