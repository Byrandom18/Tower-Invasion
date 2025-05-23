using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UpgradeEquipmentPanel : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemLevel;
    public TMP_Text mainStat;
    public TMP_Text additionalStat1;
    public TMP_Text additionalStat2;
    public TMP_Text additionalStat3;
    public TMP_Text additionalStat4;
    public List<UpgradeItemSlot> upgradeItemSlots = new List<UpgradeItemSlot>();

    private EquipmentData equipment;

    private InventoryManager inventoryManager;

    private void Start()
    {
        if (inventoryManager == null)
            inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public void SetItemToUpgrade(ItemData itemData, List<ItemSlot> itemSlots)
    {
        if (itemData == null || itemData.equipmentData == null)
            return;

        equipment = itemData.equipmentData;

        itemImage.sprite = itemData.Icon;
        itemName.text = itemData.ItemName;
        itemLevel.text = $"Уровень: {equipment.level}";
        ShowStats(equipment);

        if (equipment.upgradeRequirements != null)
            SetUpgradeRequirements(equipment.upgradeRequirements, itemSlots);
    }

    public void SetUpgradeRequirements(List<UpgradeRequirement> requirements, List<ItemSlot> itemSlots)
    {
        for (int i = 0; i < upgradeItemSlots.Count; i++)
        {
            if (i < requirements.Count)
            {
                var req = requirements[i];
                var slot = upgradeItemSlots[i];

                // Найти количество в инвентаре
                int have = 0;
                foreach (var itemSlot in itemSlots)
                {
                    if (itemSlot.item == req.itemData)
                        have += itemSlot.quantity;
                }

                slot.Set(req.itemData.Icon, have, req.amount);
                slot.item = req.itemData;
                slot.quantity = have;
            }
            else
            {
                upgradeItemSlots[i].Clear();
            }
        }
    }

    public void ShowStats(EquipmentData equipment)
    {
        // Основной стат
        if (equipment.mainStat != null)
        {
            mainStat.text = $"{equipment.mainStat.statType,-20}\t{equipment.mainStat.value,10}";
        }
        else
        {
            mainStat.text = "";
        }
        // Дополнительные статы
        if (equipment.additionalStats != null && equipment.additionalStats.Count > 0)
        {
            additionalStat1.text = equipment.additionalStats.Count > 0
            ? $"{equipment.additionalStats[0].statType,-20}\t{equipment.additionalStats[0].value,10}": "";
            additionalStat2.text = equipment.additionalStats.Count > 1
            ? $"{equipment.additionalStats[1].statType,-20}\t{equipment.additionalStats[1].value,10}": "";
            additionalStat3.text = equipment.additionalStats.Count > 2
            ? $"{equipment.additionalStats[2].statType,-20}\t{equipment.additionalStats[2].value,10}": "";
            additionalStat4.text = equipment.additionalStats.Count > 3
            ? $"{equipment.additionalStats[3].statType,-20}\t{equipment.additionalStats[3].value,10}": "";
        }
    }

    public void OnUpgradeButtonClick()
    {
        if (equipment == null)
        {
            Debug.LogWarning("Предмет для улучшения не выбран");
            return;
        }

        // Проверка наличия всех необходимых предметов
        for (int i = 0; i < equipment.upgradeRequirements.Count; i++)
        {
            var req = equipment.upgradeRequirements[i];
            var slot = upgradeItemSlots[i];

            if (slot.quantity < req.amount)
            {
                Debug.LogWarning($"Недостаточно предметов для улучшения: {slot.item.ItemName}");
                return;
            }
        }

        // Снятие ресурсов
        for (int i = 0; i < equipment.upgradeRequirements.Count; i++)
        {
            var req = equipment.upgradeRequirements[i];
            int toRemove = req.amount;

            // Снимаем предметы из инвентаря
            foreach (var itemSlot in inventoryManager.itemSlots)
            {
                if (itemSlot.item == req.itemData && toRemove > 0)
                {
                    int remove = Mathf.Min(itemSlot.quantity, toRemove);
                    itemSlot.quantity -= remove;
                    toRemove -= remove;

                    itemSlot.UpdateSlotUI();
                    if (itemSlot.quantity <= 0)
                        itemSlot.EmptySlot();
                }
            }
        }

        // Улучшение предмета
        equipment.level++;
        //equipment.GenerateStats(); // Пересчитать статы, если реализовано

        SetItemToUpgrade(inventoryManager.selectedItem, inventoryManager.itemSlots);
    }
}