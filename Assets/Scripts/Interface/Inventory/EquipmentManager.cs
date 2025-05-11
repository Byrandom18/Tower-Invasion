using Unity.Android.Gradle.Manifest;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EquipmentManager : ScriptableObject
{
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void ApplyEquipmentEffects(EquipmentData equipmentData)
    {
        if (playerStats == null || equipmentData == null) 
            return;
        ApplyStat(playerStats, equipmentData.mainStat.statType, equipmentData.mainStat.value);

        foreach (var stat in equipmentData.additionalStats)
        {
            ApplyStat(playerStats, stat.statType, stat.value);
        }
    }

    public void RemoveEquipmentEffects(EquipmentData equipmentData)
    {
        if (playerStats == null || equipmentData == null)
            return;
        ApplyStat(playerStats, equipmentData.mainStat.statType, -equipmentData.mainStat.value);
        foreach (var stat in equipmentData.additionalStats)
        {
            ApplyStat(playerStats, stat.statType, -stat.value);
        }
    }

    private void ApplyStat(PlayerStats stats, StatType statType, float value)
    {
        if (stats == null) return;

        switch (statType)
        {
            case StatType.WeaponBase: stats.weaponBase += value; break;
            case StatType.AtkModifier: stats.atkModifier += value; break;
            case StatType.AtkSpeedModifier: stats.atkSpeedModifier += value; break;
            case StatType.CritChance: stats.critChance += value; break;
            case StatType.CritDamage: stats.critDamage += value; break;
            case StatType.DefFlat: stats.defFlat += value; break;
            case StatType.HealthModifier: stats.healthModifier += value; break;
            case StatType.HealthFlat: stats.healthFlat += value; break;
            case StatType.AtkFlat: stats.atkFlat += value; break;
            case StatType.SpeedModifier: stats.speedModifier += value; break;
            case StatType.ManaRegenModifier: stats.manaRegenModifier += value; break;
            case StatType.ManaFlat: stats.manaFlat += value; break;
            case StatType.BaseAttackModifier: stats.baseAttackModifier += value; break;
            case StatType.SpdModifier: stats.spdModifier += value; break;
        }
    }

    //public bool TryAddAdditionalStat(StatType newType, float value)
    //{
    //    if (additionalStats.Count >= GetMaxAdditionalStats()) return false;
    //    if (mainStat.statType == newType || additionalStats.Exists(s => s.statType == newType))
    //        return false;

    //    additionalStats.Add(new EquipmentStat(newType, value));
    //    return true;
    //}

    //public int GetMaxAdditionalStats()
    //{
    //    return rarity switch
    //    {
    //        RarityType.Common => 1,
    //        RarityType.Rare => 2,
    //        RarityType.Epic => 3,
    //        RarityType.Legendary => 4,
    //        _ => 0
    //    };
    //}

    //public void UpgradeItem()
    //{
    //    int maxStats = GetMaxAdditionalStats();
    //}
}