using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Android.Gradle.Manifest;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EquipmentStatModifier : ScriptableObject
{
    public StatType statType;
    public float value;

    public EquipmentStatModifier(StatType type, float val)
    {
        statType = type;
        value = val;
    }

    [Header("Main Stat")]
    public EquipmentStatModifier mainStat;

    [Header("Additional Stats")]
    public List<EquipmentStatModifier> additionalStats = new List<EquipmentStatModifier>();

    public static List<EquipmentStat> GenerateAdditionalStats(RarityType rarity, StatType exclude)
    {
        int count = rarity switch
        {
            RarityType.Common => 1,
            RarityType.Rare => 2,
            RarityType.Epic => 3,
            RarityType.Legendary => 4,
            _ => 0
        };

        List<StatType> allStats = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();
        allStats.Remove(exclude);

        List<StatType> selected = allStats.OrderBy(x => UnityEngine.Random.value).Take(count).ToList();

        return selected.Select(stat => new EquipmentStat { statType = stat, value = GetRandomValue(stat) }).ToList();
    }

    private static float GetRandomValue(StatType statType)
    {
        // Example implementation for generating random values based on stat type
        return statType switch
        {
            StatType.WeaponBase => UnityEngine.Random.Range(5f, 15f),
            StatType.AtkModifier => UnityEngine.Random.Range(1.1f, 1.5f),
            StatType.AtkSpeedModifier => UnityEngine.Random.Range(0.1f, 0.3f),
            StatType.CritChance => UnityEngine.Random.Range(0.05f, 0.2f),
            StatType.CritDamage => UnityEngine.Random.Range(1.5f, 2.5f),
            StatType.DefFlat => UnityEngine.Random.Range(10f, 30f),
            StatType.HealthModifier => UnityEngine.Random.Range(1.1f, 1.5f),
            StatType.HealthFlat => UnityEngine.Random.Range(50f, 150f),
            StatType.AtkFlat => UnityEngine.Random.Range(10f, 30f),
            StatType.SpeedModifier => UnityEngine.Random.Range(1.1f, 1.3f),
            StatType.ManaRegenModifier => UnityEngine.Random.Range(0.1f, 0.5f),
            StatType.ManaFlat => UnityEngine.Random.Range(20f, 50f),
            StatType.BaseAttackModifier => UnityEngine.Random.Range(1.1f, 1.4f),
            StatType.SpdModifier => UnityEngine.Random.Range(1.1f, 1.3f),
            _ => 0f
        };
    }

    public void EquipItem()
    {
        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("PlayerStats not found during EquipItem()");
            return;
        }
        ApplyStat(stats, mainStat.statType, mainStat.value);
        foreach (var stat in additionalStats)
            ApplyStat(stats, stat.statType, stat.value);
    }

    public void UnEquipItem()
    {
        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        ApplyStat(stats, mainStat.statType, -mainStat.value);
        foreach (var stat in additionalStats)
            ApplyStat(stats, stat.statType, -stat.value);
    }

    private void ApplyStat(PlayerStats stats, StatType type, float value)
    {
        if (stats == null)
        {
            Debug.LogError("PlayerStats is NULL in ApplyStat!");
            return;
        }
        switch (type)
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
}
