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
        }
        else
        {
            itemDescriptionImage.sprite = emptySprite;
            itemDescriptionName.text = "";
            itemDescriptionText.text = "";
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
        isFull = false;
    }
}
