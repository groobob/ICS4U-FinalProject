/*
 * Handles player stats. This is a subclass of entity and manages all the background stats of the player such as damage and tempo.
 * 
 * @author Evan
 * @version January 23
 */

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


    private void Start()
    {
        firstSpawned = true;
        if (PlayerManager.Instance.isNew)
        {
            PlayerManager.Instance.isNew = false;
        }
    }

    private void Update()
    {
        if (firstSpawned)
        {
            Invoke("CheckNewUpgrades", 0.5f); // call method a little later
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
    /**
     * Updates the UI elements to reflect the current player stats.
     */
    private void UpdateUI()
    {
        //update values 
        tempoBar.value = tempo;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthBarText.text = health + "/" + maxHealth;
    }
    /**
     * Checks for new upgrades and triggers the application of upgrades if the count has changed.
     */
    private void CheckNewUpgrades()
    {
        int currentUpgradeCount = upgrades.GetComponents<Upgrade>().Length;
        if (upgrades != null && currentUpgradeCount != previousUpgradeCount)
        {
            previousUpgradeCount = currentUpgradeCount;
            PlayerManager.Instance.WorkUpgrades();
        }
    }
    /**
     * Handles the event of the player's death, triggering the appropriate actions and sounds.
     */
    public override void DeathEvent()
    {
        // INCREMENT PLAYER DEATHS HERE
        DataManager.Instance.IncrementData(DataManager.stats.deaths);
        SoundManager.Instance.PlayAudio(1);
    }
    /**
     * Increases the player's tempo within the maximum tempo limit.
     */
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
    /**
     * Spends tempo for spell usage and returns whether the operation was successful.
     * @param amount The amount of tempo to spend.
     * @return True if the tempo was successfully spent; otherwise, false.
     */
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
    /**
     * Pauses the tempo decay for the specified duration.
     * @param duration The duration for which the tempo decay is paused.
     */
    public void PauseTempoDecay(float duration)
    {
        tempoContinueTime = Time.time + duration;
    }
    /**
     * Manages the tempo decay process, reducing the tempo over time. Decreases by a specific percentage each time.
     */
    private void TempoDecay()
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
    /**
     * Processes the player taking damage and triggers appropriate actions based on the outcome. Player gains IFrames after getting hit.
     * @param damage The amount of damage taken.
     * @return bool
     */
    public new bool TakeDamage(int damage)
    {
        if (health <= 0) { SceneLoader.Instance.LoadDeathScene();  return false; } // if hitting dead
        if (!hitbox.enabled) { return false; }
        ChangeHitbox(false);
        Invoke("ChangeHitbox", 1f);

        if (health - damage <= 0 && PlayerManager.Instance.GetUpgradesPart().GetComponent<SecondSoul>())
        {
            Debug.Log("Second Soul");
            PlayerManager.Instance.GetUpgradesPart().GetComponent<SecondSoul>().UpgradeProcc();
            SoundManager.Instance.PlayAudio(7);
            return false;
        }
        else
        {
            SoundManager.Instance.PlayAudio(6);
            health -= damage;
            if (health <= 0)
            {
                DeathEvent();
            }
            return true;
        }
        
    }
    /**
     * Adds upgrades to the player, applying the effects of the added upgrade.
     * @param System.Type The type of upgrade to add.
     */
    public void AddUpgrades(System.Type upgrade)
    {
        if (upgrade.IsSubclassOf(typeof(SecondaryChange))) // If secondary change, do different behavior
        {
            foreach (SecondaryChange upg in upgrades.GetComponents<SecondaryChange>()) // delete previous secondary
            {
                Destroy(upg);
            }
            upgrades.AddComponent(upgrade);// add new one
            PlayerManager.Instance.SecondaryUpgrades(upgrade);
        }
        else
        {
            Upgrade added = upgrades.AddComponent(upgrade) as Upgrade;
        }
    }
    /**
     * Grants the player invincibility frames for the specified duration.
     * @param duration The duration of the invincibility frames.
     */
    public void GiveIFrames(float duration)
    {
        ChangeHitbox(false);
        Invoke("ChangeHitbox", duration);
    }
    /**
     * Toggles the hitbox's enabled state to the specified value.
     * @param value The new enabled state of the hitbox.
     */
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
