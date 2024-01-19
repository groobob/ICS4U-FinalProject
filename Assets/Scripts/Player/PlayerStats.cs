using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStats : Entity
{
    //Upgrades
    [SerializeField] private GameObject upgrades;
    private int previousUpgradeCount;
    //Tempo
    [SerializeField] public float tempo;
    [SerializeField] private float tempoMax;
    [SerializeField] private float tempoDecayFactor;
    [SerializeField] private float tempoDelayWait;
    [SerializeField] private float tempoGain;
    private float previousTempoTime;


    private void Start()
    {
        if (PlayerManager.Instance.isNew)
        {
            PlayerManager.Instance.isNew = false;
        }
    }

    public void Update()
    {
        CheckNewUpgrades();
        TempoDecay();
    }

    private void CheckNewUpgrades()
    {
        int currentUpgradeCount = upgrades.GetComponents<Upgrade>().Length;
        if (upgrades != null && currentUpgradeCount != previousUpgradeCount)
        {
            previousUpgradeCount = currentUpgradeCount;
            PlayerManager.Instance.WorkUpgrades();
        }
    }

    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }

    public void AddTempo()
    {
        previousTempoTime = Time.time;
        if (tempo + tempoGain > tempoMax)
        {
            tempo = tempoMax;
        }
        else
        {
            tempo += tempoGain;
        }

    }

    public bool SpendTempo(float amount) // THIS IS USED FOR SPELLS ONLY
    {
        if (tempo - amount < 0)
        {
            return false;
        }
        else
        {
            tempo -= amount;
            return true;
        }

    }

    public void TempoDecay()
    {
        if (tempo > 0 && previousTempoTime + tempoDelayWait < Time.time)
        {
            tempo *= tempoDecayFactor;
            if (tempo < 1 && tempo > 0)
            {
                tempo = 0;
            }
        }
    }

    public void AddUpgrades(System.Type upgrade)
    {
        //Upgrade added = upgrades.AddComponent(upgrade) as Upgrade;

    }
}
