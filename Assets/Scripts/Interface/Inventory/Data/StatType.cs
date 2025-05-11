using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

[System.Serializable]
public class EquipmentStat
{
    public StatType statType;
    public float value;
}