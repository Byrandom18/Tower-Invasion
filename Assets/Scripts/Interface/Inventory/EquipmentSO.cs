using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public string itemName;
    public int weaponBase, atkModifier, atkSpeedModifier, critChance,
        shieldDamageReduction, manaCostReduction,
        healthModifier, healthFlat,
        atkFlat,
        speedModifier;

    public void EquipItem()
    {
        PlayerStats playerstats = GameObject.Find("Player").GetComponent<PlayerStats>();
        //playerstats.attack += attack;
        //playerstats.defense += defense;
        //playerstats.health += health;
        //playerstats.mana += mana;

        //playerstats.UpdateEquipmentStats();
    }

    public void UnEquipItem()
    {
        PlayerStats playerstats = GameObject.Find("Player").GetComponent<PlayerStats>();
        //playerstats.attack -= attack;
        //playerstats.defense -= defense;
        //playerstats.health -= health;
        //playerstats.mana -= mana;

        //playerstats.UpdateEquipmentStats();
    }
}
