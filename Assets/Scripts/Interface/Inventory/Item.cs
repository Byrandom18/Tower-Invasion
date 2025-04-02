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

    private InventoryManager inventoryManager;

    public ItemType itemType;

    private bool isPlayerInRange = false;

    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
    //        if (leftOverItems <= 0)
    //            Destroy(gameObject);
    //        else
    //            quantity = leftOverItems;
    //    }
    //}

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
            if (leftOverItems <= 0)
                Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum ItemType { Equipment, Resource, Consumable, UpgradeItem, SpecialItem }
//public enum EquipmentType { Weapon, Armor, Helmet, Boots, Gloves, Ring1, Ring2, None }

//[System.Serializable]
//public struct StatModifier
//{
//    public float healthBonus;
//    public float attackBonus;
//    public float defenseBonus;
//}

//[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
//public class Item : ScriptableObject
//{
//    public string itemName;
//    public Sprite sprite;
//    public ItemType itemType;
//    public EquipmentType equipmentType = EquipmentType.None;
//    public int quantity = 1;
//    [TextArea]
//    public string itemDescription;
//    public bool isStackable;
//    public int maxStack = 99;


//    public StatModifier statModifier;

//    public string itemID;

//    private void OnValidate()
//    {
//        if (!isStackable)
//        {
//            maxStack = 1;
//        }

//        // Убираем EquipmentType, если предмет не экипировка
//        if (itemType != ItemType.Equipment)
//        {
//            equipmentType = EquipmentType.None;
//        }

//        // Генерация ID, если не задан
//        if (string.IsNullOrEmpty(itemID))
//        {
//            itemID = System.Guid.NewGuid().ToString();
//        }
//    }

//    public StatModifier GetStatModifiers()
//    {
//        return statModifier;
//    }

//    public Item Clone()
//    {
//        Item newItem = Instantiate(this);
//        newItem.itemID = System.Guid.NewGuid().ToString(); // Уникальный ID для копии
//        return newItem;
//    }
//}
