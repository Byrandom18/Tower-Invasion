using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryEquipPanel;
    public GameObject InventoryPanel; 
    public GameObject EquipmentPanel;
    public GameObject UpgradeItemPanel;
    public GameObject TabPanel;

    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    public List<EquippedSlot> equippedSlots = new List<EquippedSlot>();

    public ItemData selectedItem;

    public GameObject itemPickupPrefab;

    public bool isInventoryOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        Time.timeScale = 0;
        isInventoryOpen = !isInventoryOpen;
        InventoryEquipPanel.SetActive(isInventoryOpen);
        TabPanel.SetActive(isInventoryOpen);

        if (!isInventoryOpen)
        {
            Time.timeScale = 1;
            DeselectAllSlots();
        }
    }
    public void OnUpgradeButtonClick()
    {
        if (selectedItem != null)
            OpenUpgradePanel(selectedItem);
        else
            Debug.LogWarning("Предмет для улучшения не выбран.");
    }
    public void OpenUpgradePanel(ItemData selectedItem)
    {
        UpgradeItemPanel.SetActive(true);

        var upgradeEquipment = UpgradeItemPanel.GetComponent<UpgradeEquipmentPanel>();
        if (upgradeEquipment != null)
        {
            upgradeEquipment.SetItemToUpgrade(selectedItem, itemSlots);
        }
    }
    public void CloseUpgradePanel()
    {
        if (UpgradeItemPanel != null)
            UpgradeItemPanel.SetActive(false);
    }

    public int AddItem(ItemData itemToAdd, int amount)
    {
        if (itemToAdd == null)
            return amount;
        if (itemToAdd.IsEquippable())
        {
            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                if (!equipmentSlots[i].isFull && (equipmentSlots[i].item == null || equipmentSlots[i].item == itemToAdd))
                {
                    int leftOverItems = equipmentSlots[i].AddItem(itemToAdd, amount);

                    if (leftOverItems > 0)
                        return AddItem(itemToAdd, leftOverItems);

                    return 0;
                }
            }
            return amount;
        }
        else
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].isFull == false && itemSlots[i].item == itemToAdd || itemSlots[i].quantity == 0)
                {
                    int leftOverItems = itemSlots[i].AddItem(itemToAdd, amount);

                    if (leftOverItems > 0)
                        return AddItem(itemToAdd, leftOverItems);

                    return 0;
                }
            }
            return amount;
        }
    }

    public void DropItem(ItemData itemToDrop,  int amount)
    {
        if (itemToDrop == null || amount <= 0)
        {
            Debug.LogWarning("Невозможно выбросить предмет: некорректные данные.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Игрок не найден.");
            return;
        }

        Vector2 dropPosition = (Vector2)player.transform.position + Vector2.right;

        GameObject droppedItem = Instantiate(itemPickupPrefab, dropPosition, Quaternion.identity);
       

        ItemPickup pickup = droppedItem.GetComponent<ItemPickup>();

        if (pickup != null)
        {
            pickup.item = itemToDrop;
            pickup.amount = amount;
            pickup.name = itemToDrop.ItemName;
            pickup.GetComponent<SpriteRenderer>().sprite = itemToDrop.Icon;
        }
        else
        {
            Debug.LogWarning("У префаба отсутствует компонент ItemPickup.");
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].selectedShader.SetActive(false);
            itemSlots[i].thisItemSelected = false;
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            equipmentSlots[i].selectedShader.SetActive(false);
            equipmentSlots[i].thisItemSelected = false;
        }

        for (int i = 0; i < equippedSlots.Count; i++)
        {
            equippedSlots[i].selectedShader.SetActive(false);
            equippedSlots[i].thisItemSelected = false;
        }
    }
}