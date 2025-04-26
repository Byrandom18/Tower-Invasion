using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using static UnityEditor.Progress;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    //===ITEMDATA===
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public ItemType itemType;

    //===ITEM SLOT===
    [SerializeField]
    private Image itemImage;
    public GameObject selectedShader;
    public bool thisItemSelected;
    public Sprite emptySprite;

    //===ITEM Description===
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

    //===EQUIPPED SLOTS===
    [SerializeField]
    private EquippedSlot helmetSlot, armorSlot, glovesSlot, bootsSlot, weaponSlot, ring1Slot, ring2Slot;

    private InventoryManager inventoryManager;

    //===DROP ITEM===
    public GameObject dropPrefab;
    public Transform playerTransform;

    private void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (isFull)
            return quantity;

        this.itemType = itemType;

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        this.itemDescription = itemDescription;

        this.quantity = 1;
        isFull = true;

        return 0;
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

    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            EquipGear();
            EmptySlot();
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;

            itemDescriptionName.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;

            if (itemDescriptionImage.sprite == null)
                itemDescriptionImage.sprite = emptySprite;
        }

    }

    private void EquipGear()
    {
        if (itemType == ItemType.Helmet)
            helmetSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.Armor)
            armorSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.Gloves)
            glovesSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.Boots)
            bootsSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.Weapon)
            weaponSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.Ring)
        {
            if (!ring1Slot.IsInUse())
                ring1Slot.EquipGear(itemSprite, itemName, itemDescription);
            else if (!ring2Slot.IsInUse())
                ring2Slot.EquipGear(itemSprite, itemName, itemDescription);
            else
                Debug.Log("Оба слота для колец заняты.");
        }

        EmptySlot();
    }

    public void OnRightClick()
    {
        if (quantity <= 0) 
            return;

        // Сохраняем данные предмета перед очисткой
        string droppedItemName = itemName;
        Sprite droppedItemSprite = itemSprite;
        string droppedItemDescription = itemDescription;
        ItemType droppedItemType = itemType;

        GameObject itemToDrop = new GameObject(itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemName = itemName;
        newItem.sprite = itemSprite;
        newItem.itemType = itemType;
        newItem.itemDescription = itemDescription;

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;

        itemToDrop.AddComponent<CircleCollider2D>().isTrigger = true;

        float dropDistance = 1f;
        Vector2 dropPosition = (Vector2)playerTransform.position + (Vector2)playerTransform.right * dropDistance;

        itemToDrop.transform.position = (Vector2)playerTransform.position + (Vector2)playerTransform.right * dropDistance;
        itemToDrop.transform.localScale = new Vector3(.3f, .3f, .3f); //Уменьшение предмета

        this.quantity -= 1;
        if (this.quantity <= 0)
        {
            EmptySlot();
        }
    }

    private void EmptySlot()
    {
        itemName = "";
        itemSprite = null;
        itemDescription = "";
        itemType = default;
        quantity = 0;

        itemImage.sprite = emptySprite;

        itemDescriptionName.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
        isFull = false;
    }
}