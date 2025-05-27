using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentData
{
    public EquipmentType equipmentType;

    [Header("Редкость предмета")]
    public RarityType rarity;

    public int level;

    [Header("Основной стат")]
    public EquipmentStat mainStat;

    [Header("Дополнительные статы")]
    public List<EquipmentStat> additionalStats = new List<EquipmentStat>();

    [Header("Требуемые для улучшения ресурсы")]
    public List<UpgradeRequirement> upgradeRequirements = new List<UpgradeRequirement>();

    public void GenerateStats()
    {
        mainStat = GenerateMainStat(equipmentType, level);
        additionalStats = GenerateAdditionalStats(rarity);
    }

    public static EquipmentStat GenerateMainStat(EquipmentType type, int level)
    {
        StatType statType;
        switch (type)
        {
            case EquipmentType.Weapon:
                statType = StatType.WeaponBase;
                break;
            case EquipmentType.Boots:
                statType = StatType.SpeedModifier;
                break;
            case EquipmentType.Helmet:
                statType = (UnityEngine.Random.value < 0.5f) ? StatType.CritChance : StatType.CritDamage;
                break;
            default:
                StatType[] pool = { StatType.HealthModifier, StatType.AtkModifier, StatType.DefFlat };
                statType = pool[UnityEngine.Random.Range(0, pool.Length)];
                break;
        }
        float value = EquipmentStat.mainStatValues[statType][Mathf.Clamp(level - 1, 0, 4)];
        return new EquipmentStat {  statType = statType, value = value };
    }

    public static List<EquipmentStat> GenerateAdditionalStats(RarityType rarity)
    {
        int count = rarity switch
        {
            RarityType.Common => 1,
            RarityType.Rare => 2,
            RarityType.Epic => 3,
            RarityType.Legendary => 4,
            _ => 1
        };

        var possibleStats = new List<StatType>(EquipmentStat.additionalStatValues.Keys);
        var selected = new HashSet<StatType>();
        var result = new List<EquipmentStat>();

        for (int i = 0; i < count; i++)
        {
            var available = possibleStats.FindAll(s => !selected.Contains(s));
            if (available.Count == 0) break;

            var statType = available[UnityEngine.Random.Range(0, available.Count)];
            selected.Add(statType);

            result.Add(new EquipmentStat
            {
                statType = statType,
                value = EquipmentStat.additionalStatValues[statType]
            });
        }
        return result;
    }
}
