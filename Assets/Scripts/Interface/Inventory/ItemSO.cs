using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributeToChange attributeToChange = new AttributeToChange();
    public int amountTochangeAttribute;

    public enum StatToChange
    {
        none, health, mana
    };

    public enum AttributeToChange
    {
        none, strength, defense, health, mana
    };

    public bool UseItem()
    {
        //if (statToChange == StatToChange.health)
        //{
        //    PlayerHealth playerHealth = GameObject.Find("HealthManager").GetComponent<PlayerHealth>();
        //    if (playerHealth.health == playerHealth.maxHealth)
        //    {
        return false;
        //    }
        //    else
        //    {
        //        playerHealth.RestoreHealth(amountToChangeStat);
        //        return true;
        //    }

        //}

        //return false;

        //if (statToChange == StatToChange.mana)
        //{
        //    GameObject.Find("ManaManager").GetComponent<PlayerMana>().ChangeHealth(amountToChangeStat);
        //}
    }
}