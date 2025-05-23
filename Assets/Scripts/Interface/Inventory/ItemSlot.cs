using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData item { get; private set; }
    public int quantity;
    public bool isFull;

    [Header("Item Slot")]
    [SerializeField]
    private Image itemImage; 

    [SerializeField]
    private TMP_Text quantityText;

    public GameObject selectedShader;

    [Header("Item Description")]
    public Image itemDescriptionImage;  
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

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
            if (item.IsConsumable())
            {
                quantity--;
                quantityText.text = quantity.ToString();
                if (quantity <= 0)
                {
                    EmptySlot();
                }
            }
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

        if (item == null)
        {
            item = itemToAdd;
            itemImage.sprite = item.Icon;
            quantity = 0;
        }

        if (!item.IsStackable)
            return amount;

        quantity += amount;

        if (quantity > item.MaxStack)
        {
            int extraItems = quantity - item.MaxStack;
            quantity = item.MaxStack;
            isFull = true;
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
            return extraItems;
        }

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        if (quantity == item.MaxStack)
            isFull = true;

        return 0;
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

    public void UpdateSlotUI()
    {
        if (item != null && quantity > 0)
        {
            itemImage.sprite = item.Icon;
            quantityText.text = quantity.ToString();
            quantityText.enabled = item.IsStackable;
        }
        else
        {
            itemImage.sprite = emptySprite;
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }

    public void EmptySlot()
    {
        item = null;
        quantity = 0;
        quantityText.enabled = false;

        itemImage.sprite = emptySprite;
        itemDescriptionName.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
        isFull = false;
    }
}
