using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note to self. Save stats, load stats.

public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager Instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    PlayerStats _playerStats;
    GameObject upgrades;

    //Saved Stats
    private int savedHealth;
    private int savedMaxHealth;
    private float savedMovespeed;
    public bool isNew; // Determines if a player needs new base stats or not, used in PlayerStats

    //Stats
    public int addedHealth;
    public int addedMaxHealth;
    public float addedMovespeed;


    private void Awake()
    {
        Instance = this;
        isNew = true;
    }

    /**
     * Method for spawning the player, loads all data.
     * @param x X coord for spawning the player
     * @param y Y coord for spawning the player
     * @return GameObject reference to the player
     */
    public GameObject SpawnPlayer(float x, float y)
    {
        Debug.Log(x + ", " + y);
        player = Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
        _playerStats = player.GetComponent<PlayerStats>();
        upgrades = player.transform.Find("Upgrades").gameObject;

        if (isNew) { isNew = false; }
        else { LoadStats(); }

        return player;
    }

    /**
     * Method for despawning the player, saves all data.
     */
    public void DespawnPlayer()
    {
        SaveStats();
        Destroy(player);
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
        LoadUpgrades();
    }


    private void ResetCharacter()
    {
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
        _playerStats.health -= addedHealth;
        _playerStats.maxHealth -= addedMaxHealth;
        _playerStats.baseMoveSpeed -= addedMovespeed;

        addedHealth = 0;
        addedMaxHealth = 0;
        addedMovespeed = 0;
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            addedHealth += upg.healthBoost;
            addedMaxHealth += upg.healthBoost;
            addedMovespeed += upg.speedBoost;
        }
        _playerStats.health += addedHealth;
        _playerStats.maxHealth += addedMaxHealth;
        _playerStats.baseMoveSpeed += addedMovespeed;
    }

    private void WipeUpgrades()
    {
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            Destroy(upg);
        }
    }
}
