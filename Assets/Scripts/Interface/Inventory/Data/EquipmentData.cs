using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class EquipmentData
{
    public EquipmentType equipmentType;

    [Header("Редкость предмета")]
    public RarityType rarity;

    [Header("Основной стат")]
    public EquipmentStat mainStat;

    [Header("Дополнительные статы")]
    public List<EquipmentStat> additionalStats = new List<EquipmentStat>();
}
