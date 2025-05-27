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

    public bool Upgrade()
    {
        int maxLevel = rarity switch
        {
            RarityType.Common => 1,
            RarityType.Rare => 2,
            RarityType.Epic => 3,
            RarityType.Legendary => 4,
            _ => 1
        };

        if (level >= maxLevel)
            return false;

        level++;

        if (mainStat != null)
            mainStat.SetDefaultValue(level);

        if (additionalStats.Count < 4)
        {
            var possibleStats = new List<StatType>(EquipmentStat.additionalStatValues.Keys);
            var usedStats = new HashSet<StatType>(additionalStats.ConvertAll(s => s.statType));
            if (mainStat != null) usedStats.Add(mainStat.statType);

            var available = possibleStats.FindAll(s => !usedStats.Contains(s));
            if (available.Count > 0)
            {
                var statType = available[UnityEngine.Random.Range(0, available.Count)];
                var newStat = new EquipmentStat { statType = statType };
                newStat.SetDefaultValue();
                additionalStats.Add(newStat);
            }
        }

        return true;
    }
}
