using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Resource}
//public enum EquipmentType { Weapon, Armor, Helmet, Boots, Gloves, Ring }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
   // public EquipmentType equipmentType;
    public string description;
    public int maxStack = 99;
}
