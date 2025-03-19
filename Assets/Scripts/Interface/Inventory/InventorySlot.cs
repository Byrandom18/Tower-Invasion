using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler//, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public TMP_Text stackText;
    private Item item;
    private int amount = 0;
    private InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void AddItem(Item newItem, int count = 1)
    {
        item = newItem;
        amount = count;
        icon.sprite = item.icon;
        icon.enabled = true;
        stackText.text = (item.maxStack > 1) ? amount.ToString() : "";
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        amount = 0;
        stackText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {   
        //удалить на пкм
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClearSlot();
        }
        else
        {
            if (item != null)
                inventoryUI.ShowItemDetails(item);
        }
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        inventoryUI.StartDrag(this, icon.sprite);
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    inventoryUI.EndDrag();
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    if (inventoryUI.draggedSlot != null && inventoryUI.draggedSlot != this)
    //    {
    //        InventorySlot dragged = inventoryUI.draggedSlot;

    //        if (dragged.item != null)
    //        {
    //            if (dragged.item == item && item.maxStack > 1)
    //            {
    //                int total = amount + dragged.amount;
    //                int maxStack = item.maxStack;
    //                if (total <= maxStack)
    //                {
    //                    AddItem(item, total);
    //                    dragged.ClearSlot();
    //                }
    //                else
    //                {
    //                    AddItem(item, maxStack);
    //                    dragged.amount = total - maxStack;
    //                    dragged.stackText.text = dragged.amount.ToString();
    //                }
    //            }
    //            else
    //            {
    //                Item tempItem = item;
    //                int tempAmount = amount;
    //                AddItem(dragged.item, dragged.amount);
    //                dragged.AddItem(tempItem, tempAmount);
    //            }
    //        }
    //    }
    //}

    public Item GetItem() => item;
    public int GetAmount() => amount;
}