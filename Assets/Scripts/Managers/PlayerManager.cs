using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Note to self. Save stats, load stats.

public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] public Slider tempoBar;
    [SerializeField] public Slider healthBar;
    [SerializeField] public TextMeshProUGUI healthBarText;
    private GameObject player;
    PlayerStats _playerStats;
    private PlayerController _playerControl;
    private GameObject upgrades;

    public GameObject damageNumbers;

    //Saved Stats
    private int savedHealth;
    private int savedMaxHealth;
    private float savedMovespeed;
    public bool isNew; // Determines if a player needs new base stats or not, used in PlayerStats

    //Stats
    private int addedHealth;
    private int addedMaxHealth;
    private float addedMovespeed;
    private float addedRange;
    private float addedTempoGain;
    private float addedTempoMax;
    private int addedDamage;

    private System.Type currentSecondaryChange;

    [SerializeField] public Dictionary<int, Upgrade> hashMap = new Dictionary<int, Upgrade>();

    private void Awake()
    {
        Instance = this;
        isNew = true;
        /*healthBarSlider.value = savedHealth;
        healthBarText.text = savedHealth + "/" + savedMaxHealth;*/
    }
    /**
     * Method for spawning the player, loads all data.
     * @param x X coord for spawning the player
     * @param y Y coord for spawning the player
     * @return GameObject reference to the player
     */
    public GameObject SpawnPlayer(float x, float y)
    {
        //Debug.Log(x + ", " + y);
        player = Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
        _playerStats = player.GetComponent<PlayerStats>();
        _playerControl = _playerStats.gameObject.GetComponent<PlayerController>();
        upgrades = player.transform.Find("Upgrades").gameObject;

        if (isNew) { isNew = false; }
        else { LoadStats(); }

        _playerStats.tempoBar = tempoBar;
        _playerStats.healthBar = healthBar;
        _playerStats.healthBarText = healthBarText;

        return player;
    }

    public float GetAddedMoveSpeed()
    {
        return addedMovespeed;
    }

    /**
     * Method for despawning the player, saves all data.
     */
    public void DespawnPlayer()
    {
        SaveStats();
        Destroy(player);

        addedHealth = 0;
        addedMaxHealth = 0;
        addedMovespeed = 0;
        addedRange = 0;
        addedDamage = 0;
        addedTempoGain = 0;
        addedTempoMax = 0;
    }

    public void SaveStats()
    {
        savedHealth = _playerStats.GetHealth() - addedHealth;
        savedMaxHealth = _playerStats.GetMaxHealth() - addedMaxHealth;
        savedMovespeed = _playerStats.GetMoveSpeed() - addedMovespeed;
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            System.Type upgrade = upg.GetType();
            Instance.gameObject.AddComponent(upgrade);
        }
    }

    public void LoadStats()
    {
        Debug.Log("Loaded stats");
        _playerStats.health = savedHealth;
        _playerStats.maxHealth = savedMaxHealth;
        _playerStats.baseMoveSpeed = savedMovespeed;
        Debug.Log(savedHealth);
        LoadUpgrades();
    }


    private void ResetCharacter()
    {
        addedHealth = 0;
        addedMaxHealth = 0;
        addedMovespeed = 0;
        addedRange = 0;
        addedDamage = 0;
        addedTempoGain = 0;
        addedTempoMax = 0;

        isNew = true;
        WipeUpgrades();
    }

    public void LoadUpgrades()
    {
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            System.Type upgrade = upg.GetType();
            upgrades.gameObject.AddComponent(upgrade);
        }
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            Destroy(upg);
        }
    }

    public void WorkUpgrades() // makes the passive upgraes work
    {
        Debug.Log("Work Upgrades called");
        _playerStats.health -= addedHealth;
        _playerStats.maxHealth -= addedMaxHealth;
        _playerStats.baseMoveSpeed -= addedMovespeed;
        _playerStats.bonusRange -= addedRange;
        _playerStats.bonusDamage -= addedDamage;
        _playerStats.tempoGain -= addedTempoGain;
        _playerStats.tempoMax -= addedTempoMax;

        addedHealth = 0;
        addedMaxHealth = 0;
        addedMovespeed = 0;
        addedRange = 0;
        addedDamage = 0;
        addedTempoGain = 0;
        addedTempoMax = 0;
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            addedHealth += upg.healthBoost;
            addedMaxHealth += upg.healthBoost;
            addedMovespeed += upg.speedBoost;
            addedRange += upg.weaponRangeBoost;
            addedDamage += upg.damageBoost;
            addedTempoGain += upg.tempoGainBoost;
            addedTempoMax += upg.tempoMaxBoost;
        }
        _playerStats.health += addedHealth;
        _playerStats.maxHealth += addedMaxHealth;
        _playerStats.baseMoveSpeed += addedMovespeed;
        _playerStats.bonusRange += addedRange;
        _playerStats.bonusDamage += addedDamage;
        _playerStats.tempoGain += addedTempoGain;
        _playerStats.tempoMax += addedTempoMax;

        SecondaryUpgrades(currentSecondaryChange);
    }

    public void SecondaryUpgrades(System.Type secondaryChoice)
    {
        currentSecondaryChange = secondaryChoice;
        if (secondaryChoice == typeof(WindwallUpgrade))
        {
            _playerControl.UpdateSecondaryWeapon(typeof(Windwall));
        }
        else if (secondaryChoice == typeof(PhantomStep))
        {
            _playerControl.UpdateSecondaryWeapon(typeof(PhantomStep));
        }
        else if (secondaryChoice == typeof(FireColumnUpgrade))
        {
            _playerControl.UpdateSecondaryWeapon(typeof(FireColumn));
        }
    }
    public void TempUpgrades()
    {
        _playerStats.tempDmgBoost = 0;
        

        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            _playerStats.tempDmgBoost += upg.tempDmg;
        }
    }

    private void WipeUpgrades()
    {
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            Destroy(upg);
        }
    }

    public void ResetTempoBurstCD()
    {
        _playerStats.gameObject.GetComponent<PlayerController>().tempoCDTime = 0;
    }

    public List<Upgrade> GetUpgradesList()
    {
        //Upgrade[] upgList = new Upgrade[gameObject.GetComponents(typeof(Upgrade)).Length];
        List<Upgrade> upgList = new List<Upgrade>();
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            upgList.Add(upg);
        }

        return upgList;
    }

    public GameObject GetUpgradesPart()
    {
        return upgrades;
    }
}
