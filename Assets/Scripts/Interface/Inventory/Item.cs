using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public int quantity;
    [SerializeField]
    public Sprite sprite;
    [TextArea]
    [SerializeField]
    public string itemDescription;

    public ItemType itemType;

    private InventoryManager inventoryManager;

    private bool isPlayerInRange = false;

    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    private void Update()
    {

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) &&
            itemType != ItemType.Resource &&
            itemType != ItemType.Consumable &&
            itemType != ItemType.UpgradeItem)
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
            playerHealth.AddPotion();
            }

        if (itemType == ItemType.Resource ||
            itemType == ItemType.Consumable ||
            itemType == ItemType.UpgradeItem)
        {
            PickUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }

    private void PickUp()
    {
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
        
            if (leftOverItems <= 0)
            Destroy(gameObject);
        else
            quantity = leftOverItems;
    }
}