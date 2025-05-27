using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    private bool playerInRange = false;
    private bool isPickedUp = false;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && item.ItemType == ItemType.Equipment && !isPickedUp)
        {
            Pickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && item.ItemType == ItemType.Consumable && !isPickedUp)
            {
                playerHealth.AddPotion();
                Pickup();
                isPickedUp = true;
                Destroy(gameObject); 
                return;
            }

            if ((item.ItemType == ItemType.Resource || item.ItemType == ItemType.UpgradeItem) && !isPickedUp)
            {
                Pickup();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void Pickup()
    {
        if (isPickedUp) return;

        int leftOverItems = inventoryManager.AddItem(item, amount);

        if (leftOverItems <= 0)
        {
            isPickedUp = true;
            Destroy(gameObject);
        }
        else
        {
            amount = leftOverItems;
        }
    }
}