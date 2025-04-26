//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//[CreateAssetMenu]
//public class EquipmentSO : ScriptableObject
//{
//    public string itemName;
//    public int weaponBase, atkModifier, atkSpeedModifier, critChance,
//        shieldDamageReduction, manaCostReduction,
//        healthModifier, healthFlat,
//        atkFlat,
//        speedModifier;

//    public void EquipItem()
//    {
//        PlayerStats playerstats = GameObject.Find("Player").GetComponent<PlayerStats>();
//        //playerstats.attack += attack;
//        //playerstats.defense += defense;
//        //playerstats.health += health;
//        //playerstats.mana += mana;

//        //playerstats.UpdateEquipmentStats();
//    }

//    public void UnEquipItem()
//    {
//        PlayerStats playerstats = GameObject.Find("Player").GetComponent<PlayerStats>();
//        //playerstats.attack -= attack;
//        //playerstats.defense -= defense;
//        //playerstats.health -= health;
//        //playerstats.mana -= mana;

//        //playerstats.UpdateEquipmentStats();
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    WeaponBase,
    AtkModifier,
    AtkSpeedModifier,
    CritChance,
    CritDamage,
    DefFlat,
    HealthModifier,
    HealthFlat,
    AtkFlat,
    SpeedModifier,
    ManaRegenModifier,
    ManaFlat,
    BaseAttackModifier,
    SpdModifier
}
public enum RarityType
{
    Common,     // 1 доп стат
    Rare,       // 2 доп стата
    Epic,       // 3 доп стата
    Legendary   // 4 доп стата
}


[System.Serializable]
public class EquipmentStat
{
    public StatType statType;
    public float value;

    public EquipmentStat(StatType type, float val)
    {
        statType = type;
        value = val;
    }
}

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Inventory/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public string itemName;

    public RarityType rarity;

    [Header("Main Stat")]
    public EquipmentStat mainStat;

    [Header("Additional Stats")]
    public List<EquipmentStat> additionalStats = new List<EquipmentStat>();

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

    public bool TryAddAdditionalStat(StatType newType, float value)
    {
        if (additionalStats.Count >= GetMaxAdditionalStats()) return false;
        if (mainStat.statType == newType || additionalStats.Exists(s => s.statType == newType))
            return false;

        additionalStats.Add(new EquipmentStat(newType, value));
        return true;
    }

    public int GetMaxAdditionalStats()
    {
        return rarity switch
        {
            RarityType.Common => 1,
            RarityType.Rare => 2,
            RarityType.Epic => 3,
            RarityType.Legendary => 4,
            _ => 0
        };
    }

    public void UpgradeItem()
    {
        int maxStats = GetMaxAdditionalStats();
    }
}
