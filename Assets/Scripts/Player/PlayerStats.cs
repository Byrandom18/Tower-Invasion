using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //UI
    public Slider healthBar;
    public Slider manaBar;

    //stats
    public float maxHealth = 10;
    public float health;
    public float healthModifier;
    public float healthFlat;
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
    public float mana;

    public float critChance;
    public float critDamage;
    public float defFlat;

    public float speedModifier;

    public float atkSpeedModifier;
    public float spdModifier;
    public float baseAttackModifier;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateEquipmentStats();
        health = maxHealth;
        mana = maxMana;
        BarsUpdate();
    }

    public void UpdateEquipmentStats()
    {
        maxHealth = (healthBase * (1 + healthModifier)) + healthFlat;
        atk = ((baseAttack + weaponBase) * (1 + atkModifier)) + atkFlat;
        maxMana = manaBase + manaFlat;
        BarsUpdate();
    }

    public void TakeDamage(float damage)
    {
        
        health -= damage;
        healthBar.value = health;
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // update health and mana bars
    public void BarsUpdate()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        manaBar.maxValue = maxMana;
        manaBar.value = mana;
    }
}
