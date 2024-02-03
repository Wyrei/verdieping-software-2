using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : MonoBehaviour
{
    public float nextLvlXp;
    public float Xp;
    
    public float HPMax;
    public float CurrentHP;
    
    public float MaxMana;
    public float CurrentMana;

    public int Damage;
    public int DamageCalc;
    public float Speed;
    public int Dexterity;
    public int Intellegians;

    public float HPDexterity;
    public float ManaIntelegians;

    private bool IsMaxHp = true;
    void Update()
    {
        HPMax = Dexterity * HPDexterity * 2 / 1;
        MaxMana = Intellegians * ManaIntelegians * 5 / 1;
        Damage = DamageCalc * 3;
        HPMax = Mathf.Round(HPMax);
        MaxMana = Mathf.Round(MaxMana);
        if (IsMaxHp == true)
        {
            CurrentMana = MaxMana;
            CurrentHP = HPMax;
            IsMaxHp = false;
        }

        if (Xp >= nextLvlXp)
        {
            levelUp();
        }
    }

    void levelUp()
    {
        Dexterity += 1;
        CurrentMana = MaxMana;
        CurrentHP = HPMax;
        Xp -= nextLvlXp;
        nextLvlXp *= 3.212f;
    }

    public void Healing(float healingAmount)
    {
        CurrentHP += healingAmount;
    }
}
