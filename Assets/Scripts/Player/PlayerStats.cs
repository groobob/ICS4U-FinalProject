using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Entity
{
    //Upgrades
    [SerializeField] private GameObject upgrades;
    private int previousUpgradeCount;
    //Tempo
    [SerializeField] public float tempo;
    [SerializeField] public float tempoMax;
    [SerializeField] private float tempoDecayFactor;
    [SerializeField] private float tempoDelayWait;
    [SerializeField] public float tempoGain;
    private float previousTempoTime;
    private float tempoContinueTime;

    [SerializeField] private CircleCollider2D hitbox;
    // References
    public Slider tempoBar;
    public Slider healthBar;
    public TextMeshProUGUI healthBarText;

    //Upgrades
    public float bonusRange;
    public int bonusDamage;

    // Temp Boosts

    public int tempDmgBoost;

    private bool firstSpawned;


    private new void Start()
    {
        firstSpawned = true;
        if (PlayerManager.Instance.isNew)
        {
            PlayerManager.Instance.isNew = false;
        }
    }

    public void Update()
    {
        if (firstSpawned)
        {
            Invoke("CheckNewUpgrades", 0.5f);
        }
        else
        {
            CheckNewUpgrades();
        }

        //CheckNewUpgrades()
        TempoDecay();
        UpdateUI();
        PlayerManager.Instance.TempUpgrades();
    }

    public void UpdateUI()
    {
        tempoBar.value = tempo;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthBarText.text = health + "/" + maxHealth;
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

    public void PauseTempoDecay(float duration)
    {
        tempoContinueTime = Time.time + duration;
    }

    public void TempoDecay()
    {
        if (tempoContinueTime > Time.time) { return; }

        if (tempo > 0 && previousTempoTime + tempoDelayWait < Time.time)
        {
            tempo *= tempoDecayFactor;
            if (tempo < 1 && tempo > 0)
            {
                tempo = 0;
            }
        }
    }

    public new bool TakeDamage(int damage)
    {
        if (health <= 0) { return false; } // if hitting dead
        if (!hitbox.enabled) { return false; }
        ChangeHitbox(false);
        Invoke("ChangeHitbox", 1.5f);
        health -= damage;
        if (health <= 0)
        {
            DeathEvent();
        }
        return true;
    }

    public void AddUpgrades(System.Type upgrade)
    {
        //Upgrade added = upgrades.AddComponent(upgrade) as Upgrade;

        if (upgrade.IsSubclassOf(typeof(SecondaryChange)))
        {
            foreach (SecondaryChange upg in upgrades.GetComponents<SecondaryChange>())
            {
                Destroy(upg);
            }
            upgrades.AddComponent(upgrade);
            PlayerManager.Instance.SecondaryUpgrades(upgrade);
        }

    }

    public void GiveIFrames(float duration)
    {
        ChangeHitbox(false);
        Invoke("ChangeHitbox", duration);
    }

    private void ChangeHitbox(bool value)
    {
        hitbox.enabled = value;
        //Debug.Log("Hitbox changed");
    }
    private void ChangeHitbox()
    {
        hitbox.enabled = true;
        //Debug.Log("Hitbox True");
    }
}
