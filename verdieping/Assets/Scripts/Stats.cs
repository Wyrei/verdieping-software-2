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

    public int Attack;
    public float Speed;
    public int Dexterity;
    public int Wisdom;
    public int Intellegians;

    public float HPDexterity;
    public float ManaIntelegians;

    private void Start()
    {
        CurrentHP = HPMax;
        CurrentMana = MaxMana;
    }

    void Update()
    {
        HPMax = Dexterity * HPDexterity * 2 / 1;
        MaxMana = Intellegians * ManaIntelegians * 5 / 1;
        HPMax = Mathf.Round(HPMax);
        MaxMana = Mathf.Round(MaxMana);
    }
}
