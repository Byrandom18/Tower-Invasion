using System;
using UnityEngine;

[Serializable]
public class EquipmentStatModifier
{
    public StatType statType;
    public float value;

    public EquipmentStatModifier(StatType type, float val)
    {
        statType = type;
        value = val;
    }

    public void Apply(PlayerStats stats)
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

    public void Remove(PlayerStats stats)
    {
        if (stats == null) return;
        switch (statType)
        {
            case StatType.WeaponBase: stats.weaponBase -= value; break;
            case StatType.AtkModifier: stats.atkModifier -= value; break;
            case StatType.AtkSpeedModifier: stats.atkSpeedModifier -= value; break;
            case StatType.CritChance: stats.critChance -= value; break;
            case StatType.CritDamage: stats.critDamage -= value; break;
            case StatType.DefFlat: stats.defFlat -= value; break;
            case StatType.HealthModifier: stats.healthModifier -= value; break;
            case StatType.HealthFlat: stats.healthFlat -= value; break;
            case StatType.AtkFlat: stats.atkFlat -= value; break;
            case StatType.SpeedModifier: stats.speedModifier -= value; break;
            case StatType.ManaRegenModifier: stats.manaRegenModifier -= value; break;
            case StatType.ManaFlat: stats.manaFlat -= value; break;
            case StatType.BaseAttackModifier: stats.baseAttackModifier -= value; break;
            case StatType.SpdModifier: stats.spdModifier -= value; break;
        }
    }
}