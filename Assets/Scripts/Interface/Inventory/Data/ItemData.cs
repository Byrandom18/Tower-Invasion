using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Основная информация")]
    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [TextArea]
    [SerializeField] private string description;
    public string Description => description;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    [Header("Тип предмета")]
    [SerializeField] private ItemType itemType;
    public ItemType ItemType => itemType;

    [Header("Настройки стака")]
    [SerializeField] private bool isStackable = true;
    public bool IsStackable => isStackable;

    [SerializeField] private int maxStack = 99;
    public int MaxStack => maxStack;

    [Header("Дополнительные данные")]
    public EquipmentData equipmentData;
    public ConsumableData consumableData;

    public bool IsEquippable() => itemType == ItemType.Equipment && equipmentData != null;

    public bool IsConsumable() => itemType == ItemType.Consumable && consumableData != null;
}

