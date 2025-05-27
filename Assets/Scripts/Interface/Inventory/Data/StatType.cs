using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum StatType
{
    WeaponBase,         // Базовая атака (только оружие)
    SpeedModifier,      // Скорость передвижения % (только сапоги)
    CritChance,         // Шанс критического урона (только шлем и как доп)
    CritDamage,         // Крит урон (только шлем и как доп)
    HealthModifier,     // ХП % (мейн и доп)
    AtkModifier,        // Атака % (мейн и доп)
    DefFlat,            // Защита (мейн и доп)
    HealthFlat,         // ХП (только доп)
    AtkFlat,            // Атака (только доп)
    AtkSpeedModifier,   // Скорость атаки % (только доп)
    ManaRegenModifier,  // Восстановление маны % (только доп)
    ManaFlat,           // Мана (только доп)
    SpdModifier,        // Усиление заклинаний % (только доп)
    BaseAttackModifier  // Усиление базовых атак % (только доп)
}

[System.Serializable]
public class EquipmentStat
{
    public StatType statType;
    public float value;

    public static readonly Dictionary<StatType, float[]> mainStatValues = new()
    {
        { StatType.WeaponBase,      new float[] { 10, 12, 15, 19, 25 }},
        { StatType.SpeedModifier,   new float[] { 5, 10, 15, 20, 25 }},
        { StatType.CritChance,      new float[] { 19, 23, 28, 34, 40 }},
        { StatType.CritDamage,      new float[] { 38, 45, 56, 68, 80 }},
        { StatType.HealthModifier,  new float[] { 20, 25, 32, 41, 52 }},
        { StatType.AtkModifier,     new float[] { 20, 25, 32, 41, 52 }},
        { StatType.DefFlat,         new float[] { 3, 4 , 5, 6, 8 }}
    };

    public static readonly Dictionary<StatType, float> additionalStatValues = new()
    {
        { StatType.CritChance,          10},
        { StatType.CritDamage,          20},
        { StatType.HealthModifier,      10},
        { StatType.AtkModifier,         12},
        { StatType.DefFlat,             1},
        { StatType.HealthFlat,          12},
        { StatType.AtkFlat,             3},
        { StatType.AtkSpeedModifier,    12},
        { StatType.ManaRegenModifier,   15},
        { StatType.ManaFlat,            15},
        { StatType.SpdModifier,         12},
        { StatType.BaseAttackModifier,  12}
    };

    public void SetDefaultValue(int level = 1)
    {
        if (mainStatValues.ContainsKey(statType))
        {
            int index = Math.Clamp(level, 0, mainStatValues[statType].Length - 1);
            value = mainStatValues[statType][index];
        }
        else if (additionalStatValues.ContainsKey(statType))
        {
            value = additionalStatValues[statType];
        }
        else
        {
            value = 0;
        }
    }
}