/*
 * Class to manage all things related to the player. Saving and loading stats, getting references to player, etc.
 * 
 * @author Evan
 * @version January 23
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] public Slider tempoBar;
    [SerializeField] public Slider healthBar;
    [SerializeField] public TextMeshProUGUI healthBarText;
    public GameObject player;
    public PlayerStats _playerStats;
    public PlayerController _playerControl;
    private GameObject upgrades;

    public GameObject damageNumbers;

    // Animation Gameobjects
    [SerializeField] public GameObject[] animations;

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

    private int dataHP;
    private float dataSpeed;
    private float dataTempoGain;

    private System.Type currentSecondaryChange;

    [SerializeField] public Dictionary<int, Upgrade> hashMap = new Dictionary<int, Upgrade>();

    private void Awake()
    {
        Instance = this;
        isNew = true;
        /*healthBarSlider.value = savedHealth;
        healthBarText.text = savedHealth + "/" + savedMaxHealth;*/

    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(1);
    }

    /**
     * Method for spawning the player, loads all data.
     * @param x X coord for spawning the player
     * @param y Y coord for spawning the player
     * @return GameObject Reference to the player object created
     */
    public GameObject SpawnPlayer(float x, float y)
    {
        //Debug.Log(x + ", " + y);
        // Create a player at a position, give it all the needed scripts
        player = Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
        _playerStats = player.GetComponent<PlayerStats>();
        _playerControl = _playerStats.gameObject.GetComponent<PlayerController>();
        upgrades = player.transform.Find("Upgrades").gameObject;

        dataHP = DataManager.Instance.GetShop(DataManager.upgrade.health);
        dataSpeed = DataManager.Instance.GetShop(DataManager.upgrade.speed);
        dataTempoGain = 15; // DataManager.Instance.GetShop(DataManager.upgrade.tempoGain);

        if (isNew) { 
            isNew = false;
            ApplyMenuUpgrades();
            Debug.Log("Ran");
        }
        else { LoadStats(); }
        // load bars
        _playerStats.tempoBar = tempoBar;
        _playerStats.healthBar = healthBar;
        _playerStats.healthBarText = healthBarText;


        return player;
    }
    /**
    * Method for getting movespeed.
    * @return float
    */
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

        // Reset added stats;
        addedHealth = 0;
        addedMaxHealth = 0;
        addedMovespeed = 0;
        addedRange = 0;
        addedDamage = 0;
        addedTempoGain = 0;
        addedTempoMax = 0;
    }
    /**
    * Method for saving playerstats
    */
    public void SaveStats()
    {
        // Save base stats
        savedHealth = _playerStats.GetHealth() - addedHealth;
        savedMaxHealth = _playerStats.GetMaxHealth() - addedMaxHealth;
        savedMovespeed = _playerStats.GetMoveSpeed() - addedMovespeed;
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>()) // remove all upgrades
        {
            System.Type upgrade = upg.GetType();
            Instance.gameObject.AddComponent(upgrade);
        }
    }
    /**
    * Loads saved stats and upgrades the player's stats accordingly.
    */
    public void LoadStats()
    {
        Debug.Log("Loaded stats");
        _playerStats.health = savedHealth;
        _playerStats.maxHealth = savedMaxHealth;
        _playerStats.baseMoveSpeed = savedMovespeed;
        Debug.Log(savedHealth);
        LoadUpgrades();
    }

    /**
    * Resets the character's added stats and upgrades, setting isNew to true.
    */
    private void ResetCharacter()
    {
        // reset all settings
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
    /**
    * Loads upgrades and applies their effects to the player.
    */
    public void LoadUpgrades()
    {
        // Move upgrades from player manager to the player itself.
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            System.Type upgrade = upg.GetType();
            upgrades.gameObject.AddComponent(upgrade);
            Destroy(upg);
        }
    }
    private void ApplyMenuUpgrades()
    {
        
        _playerStats.health += dataHP;
        _playerStats.maxHealth += dataHP;
        _playerStats.tempoGain += dataTempoGain;
        _playerStats.baseMoveSpeed += dataSpeed;
    }


    /**
    * Applies the effects of upgrades to the player's stats.
    */
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
        // Give upgrade boosts
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
        // secondary upgrades
        SecondaryUpgrades(currentSecondaryChange);
    }
    /**
     * Updates the player's secondary weapon based on the chosen upgrade.
     * @param secondaryChoice The type of secondary upgrade chosen.
     */
    public void SecondaryUpgrades(System.Type secondaryChoice)
    {
        currentSecondaryChange = secondaryChoice; // hardcoded to give specfic secondaries.
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
    /**
     * Applies temporary upgrades to the player's stats.
     */
    public void TempUpgrades()
    {
        _playerStats.tempDmgBoost = 0;
        
        // calculates temporary stat changes due to upgrades
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            _playerStats.tempDmgBoost += upg.tempDmg;
        }
    }
    /**
     * Removes all upgrades from the player.
     */
    private void WipeUpgrades()
    {
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            Destroy(upg);
        }
    }
    /**
     * Resets the tempo burst cooldown for the player.
     */
    public void ResetTempoBurstCD()
    {
        _playerStats.gameObject.GetComponent<PlayerController>().tempoCDTime = 0;
    }
    /**
     * Retrieves a list of upgrades attached to the player.
     * @return List<Upgrade>
     */
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
    /**
     * Retrieves the GameObject containing the upgrades.
     * @return GameObject containing the upgrades.
     */
    public GameObject GetUpgradesPart()
    {
        return upgrades;
    }
    /**
     * Disables player controls by disabling the PlayerController and Weapons components.
     */
    public void DisablePlayerControls()
    {
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponentInChildren<PlayerCamera>().enabled = false;
        _playerControl.StopPlayer();
    }
    /**
     * Enables player controls by enabling the PlayerController and Weapons components.
     */
    public void EnablePlayerControls()
    {
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Weapons>().enabled = true;
    }
}
