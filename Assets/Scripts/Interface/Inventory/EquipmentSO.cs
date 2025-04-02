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
    public int attack, defense, health, mana;

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
