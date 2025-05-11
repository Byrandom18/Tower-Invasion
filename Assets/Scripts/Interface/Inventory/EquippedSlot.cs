using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData item { get; private set; }
    public bool isFull;
    public bool slotInUse;

    [Header("EquipmentSlot")]
    [SerializeField]
    private Image itemImage;
    public GameObject selectedShader;
    
    [Header("EquipmentDescription")]
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

    public Sprite emptySprite;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        equipmentManager = ScriptableObject.CreateInstance<EquipmentManager>();
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
        if (thisItemSelected && slotInUse)
        {
            UnEquip();
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
        UnEquip();
    }

    public void Equip(ItemData itemToEquip)
    {
        if (slotInUse)
        {
            UnEquip();
        }

        item = itemToEquip;
        itemImage.sprite = item.Icon;
        slotInUse = true;

        ToShowDescription();

        if (itemToEquip.equipmentData != null)
        {
            equipmentManager.ApplyEquipmentEffects(itemToEquip.equipmentData);
        }
    }

    public void UnEquip()
    {
        if (!slotInUse)
            return;
        if (item != null && item.equipmentData != null)
            equipmentManager.RemoveEquipmentEffects(item.equipmentData);
        
        inventoryManager.AddItem(item, 1);
        item = null;
        EmptySlot();
        slotInUse = false;
        
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
        itemImage.sprite = emptySprite;
        itemDescriptionName.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }

    public bool IsInUse()
    {
        return slotInUse;
    }
}

