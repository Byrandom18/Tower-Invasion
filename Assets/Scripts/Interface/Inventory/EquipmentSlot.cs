using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData item { get; private set; }
    public int quantity;
    public bool isFull;

    [Header("EquipmentSlot")]
    [SerializeField]
    private Image itemImage;
    public GameObject selectedShader;

    [Header("EquipmentDescription")]
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;
    public TMP_Text rarityText;
    public TMP_Text mainStatDesc;
    public TMP_Text additionalStatDesc1;
    public TMP_Text additionalStatDesc2;
    public TMP_Text additionalStatDesc3;
    public TMP_Text additionalStatDesc4;

    [Header("EquippedSlots")]
    [SerializeField]
    private EquippedSlot helmetSlot, armorSlot, glovesSlot,
        bootsSlot, weaponSlot, ring1Slot, ring2Slot;

    public Sprite emptySprite;

    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    private void OnLeftClick()
    {
        if (thisItemSelected)
        {
            EquipGear();
            EmptySlot();
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            thisItemSelected = true;
            selectedShader.SetActive(true);

            ToShowDescription();
        }
    }

    private void OnRightClick()
    {
        if (inventoryManager != null)
        {
            inventoryManager.DropItem(item, quantity);
            EmptySlot();
        }
        else
        {
            Debug.LogError("InventoryManager is not assigned or found.");
        }
    }

    public int AddItem(ItemData itemToAdd, int amount)
    {
        if (isFull)
            return amount;

        item = itemToAdd;
        itemImage.sprite = item.Icon;

        quantity = 1;
        isFull = true;

        return 0;
    }

    private void EquipGear()
    {
        if (item == null || !item.IsEquippable())
            Debug.Log("лИБО Item == null либо не экипировка тип"); ;

        if (item.equipmentData.equipmentType == EquipmentType.Helmet)
            helmetSlot.Equip(item);
        if (item.equipmentData.equipmentType == EquipmentType.Armor)
            armorSlot.Equip(item);
        if (item.equipmentData.equipmentType == EquipmentType.Gloves)
            glovesSlot.Equip(item);
        if (item.equipmentData.equipmentType == EquipmentType.Boots)
            bootsSlot.Equip(item);
        if (item.equipmentData.equipmentType == EquipmentType.Weapon)
            weaponSlot.Equip(item);
        if (item.equipmentData.equipmentType == EquipmentType.Ring)
        {
            if (!ring1Slot.IsInUse())
                ring1Slot.Equip(item);
            else if (!ring2Slot.IsInUse())
                ring2Slot.Equip(item);
            else
                Debug.Log("Оба слота для колец заняты.");
        }
        EmptySlot();
    }
    private void ToShowDescription()
    {
        if (item != null)
        {
            itemDescriptionImage.sprite = item.Icon;
            itemDescriptionName.text = item.ItemName;
            itemDescriptionText.text = item.Description;

            if (itemDescriptionImage.sprite == null)
                itemDescriptionImage.sprite = emptySprite;

            if (item.IsEquippable() && item.equipmentData != null)
            {
                rarityText.text = item.equipmentData.rarity.ToString();
                // Основной стат
                if (item.equipmentData.mainStat != null)
                {
                    mainStatDesc.text = $"{item.equipmentData.mainStat.statType, -20}\t{item.equipmentData.mainStat.value, 10}";
                }
                else
                {
                    mainStatDesc.text = "";
                }
                // Дополнительные статы
                if (item.equipmentData.additionalStats != null && item.equipmentData.additionalStats.Count > 0)
                {
                    additionalStatDesc1.text = item.equipmentData.additionalStats.Count > 0
                        ? $"{item.equipmentData.additionalStats[0].statType, -20}\t{item.equipmentData.additionalStats[0].value, 10}"
                        : "";
                    additionalStatDesc2.text = item.equipmentData.additionalStats.Count > 1
                        ? $"{item.equipmentData.additionalStats[1].statType, -20}\t{item.equipmentData.additionalStats[1].value, 10}"
                        : "";
                    additionalStatDesc3.text = item.equipmentData.additionalStats.Count > 2
                        ? $"{item.equipmentData.additionalStats[2].statType,-20}\t{item.equipmentData.additionalStats[2].value,10}"
                        : "";
                    additionalStatDesc4.text = item.equipmentData.additionalStats.Count > 3
                        ? $"{item.equipmentData.additionalStats[3].statType,-20}\t{item.equipmentData.additionalStats[3].value,10}"
                        : "";
                }
                else
                {
                    additionalStatDesc1.text = "";
                    additionalStatDesc2.text = "";
                    additionalStatDesc3.text = "";
                    additionalStatDesc4.text = "";
                }
            }
            else
            {
                mainStatDesc.text = "";
                additionalStatDesc1.text = "";
                additionalStatDesc2.text = "";
                additionalStatDesc3.text = "";
                additionalStatDesc4.text = "";
            }
        }
        else
        {
            itemDescriptionImage.sprite = emptySprite;
            itemDescriptionName.text = "";
            itemDescriptionText.text = "";
            rarityText.text = "";
            mainStatDesc.text = "";
            additionalStatDesc1.text = "";
            additionalStatDesc2.text = "";
            additionalStatDesc3.text = "";
            additionalStatDesc4.text = "";
        }
    }

    private void EmptySlot()
    {
        item = null;
        quantity = 0;

        itemImage.sprite = emptySprite;
        itemDescriptionName.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
        rarityText.text = "";
        mainStatDesc.text = "";
        additionalStatDesc1.text = "";
        additionalStatDesc2.text = "";
        additionalStatDesc3.text = "";
        additionalStatDesc4.text = "";
        isFull = false;
    }
}
