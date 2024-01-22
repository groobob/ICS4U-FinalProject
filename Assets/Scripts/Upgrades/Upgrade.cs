using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{

    [SerializeField] public int healthBoost;
    [SerializeField] public float speedBoost;
    [SerializeField] public float weaponRangeBoost;
    [SerializeField] public int damageBoost;
    [SerializeField] public float tempoGainBoost;
    [SerializeField] public float tempoMaxBoost;


    [SerializeField] public string description;
    [SerializeField] public string upgradeName;
    [SerializeField] protected string classification;

    [SerializeField] public int tempDmg;

    // all upgrades need to be able ot be counted, have their base stats applied, 
    // Stat boost
    // On Attack
    // On damage taken
    // On Hit
    // On Kill
    // Active (optional)
}
