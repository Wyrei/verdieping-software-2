using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : MonoBehaviour
{

    public float HPMax;
    public float CurrentHP;
    
    public float MaxMana;
    public float CurrentMana;

    public int Damage;
    public int DamageCalc;
    public float Speed;
    public int Dexterity;
    public int Wisdom;
    public int Intellegians;

    public float HPDexterity;
    public float ManaIntelegians;

    void Start()
    {
        
    }

    void Update()
    {
        HPMax = Dexterity * HPDexterity * 2 / 1;
        MaxMana = Intellegians * ManaIntelegians * 5 / 1;
        Damage = DamageCalc * 3;
        HPMax = Mathf.Round(HPMax);
        MaxMana = Mathf.Round(MaxMana);
        CurrentHP = HPMax;
        CurrentMana = MaxMana;
    }
}
