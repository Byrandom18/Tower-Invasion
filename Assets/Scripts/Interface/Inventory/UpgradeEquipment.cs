using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UpgradeEquipment : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text mainStat;
    public TMP_Text additionalStat1;
    public TMP_Text additionalStat2;
    public TMP_Text additionalStat3;
    public TMP_Text additionalStat4;
    public List<UpgradeItemSlot> upgradeItemSlots = new List<UpgradeItemSlot>();

    public EquipmentData equipment;
    public ItemData upgradeItem;

    public void SetItemToUpgrade(ItemData equipment, List<ItemSlot> itemSlots)
    {
        if (equipment == null || equipment.equipmentData == null)
            return;
        itemImage.sprite = equipment.Icon;
        itemName.text = equipment.ItemName;
        ToShowStats(equipment);

        if (equipment.equipmentData.upgradeRequirements != null)
            SetUpgradeRequirements(equipment.equipmentData.upgradeRequirements, itemSlots);
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
            }
            else
            {
                upgradeItemSlots[i].Clear();
            }
        }
    }

    public void ToShowStats(ItemData equipment)
    {
        // Основной стат
        if (equipment.equipmentData.mainStat != null)
        {
            mainStat.text = $"{equipment.equipmentData.mainStat.statType,-20}\t{equipment.equipmentData.mainStat.value,10}";
        }
        else
        {
            mainStat.text = "";
        }
        // Дополнительные статы
        if (equipment.equipmentData.additionalStats != null && equipment.equipmentData.additionalStats.Count > 0)
        {
            additionalStat1.text = equipment.equipmentData.additionalStats.Count > 0
            ? $"{equipment.equipmentData.additionalStats[0].statType,-20}\t{equipment.equipmentData.additionalStats[0].value,10}"
                : "";
            additionalStat2.text = equipment.equipmentData.additionalStats.Count > 1
            ? $"{equipment.equipmentData.additionalStats[1].statType,-20}\t{equipment.equipmentData.additionalStats[1].value,10}"
                : "";
            additionalStat3.text = equipment.equipmentData.additionalStats.Count > 2
            ? $"{equipment.equipmentData.additionalStats[2].statType,-20}\t{equipment.equipmentData.additionalStats[2].value,10}"
                : "";
            additionalStat4.text = equipment.equipmentData.additionalStats.Count > 3
            ? $"{equipment.equipmentData.additionalStats[3].statType,-20}\t{equipment.equipmentData.additionalStats[3].value,10}"
                : "";
        }
        else
        {
            additionalStat1.text = "";
            additionalStat2.text = "";
            additionalStat3.text = "";
            additionalStat4.text = "";
        }
    }
}