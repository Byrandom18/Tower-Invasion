using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 10;
    public float health;
    public float healthModifier;
    public float heathFlat;
    public float healthBase = 100;

    public float atk;
    public float atkModifier;
    public float weaponBase;
    public float baseAttack = 10;
    public float atkFlat;

    public float manaBase = 100;
    public float maxMana;
    public float manaFlat;
    public float manaRegenModifier;


    public float critChance;
    public float critDamage;
    public float defFlat;


    public float speedModifier;
    public float spdModifier;
    public float baseAttackModifier;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void UpdateEquipmentStats()
    {
        maxHealth = (healthBase * (1 + healthModifier)) + heathFlat;
        atk = ((baseAttack + weaponBase) * (1 + atkModifier)) + atkFlat;
        maxMana = manaBase + manaFlat;


    }

    public void TakeDamage(float damage)
    {
        
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
