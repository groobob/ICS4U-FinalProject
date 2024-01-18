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
    PlayerStats playerStats;
    GameObject upgrades;

    //Saved Stats
    private int savedHealth;
    private int savedMaxHealth;
    private float savedMovespeed;
    public bool isNew; // Determines if a player needs new base stats or not, used in PlayerStats

    private void Awake()
    {
        Instance = this;
        isNew = true;
    }

    public void Start()
    {

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
        playerStats = player.GetComponent<PlayerStats>();
        upgrades = player.transform.Find("Upgrades").gameObject;

        LoadStats();
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
        savedHealth = playerStats.GetHealth();
        savedMaxHealth = playerStats.GetMaxHealth();
        savedMovespeed = playerStats.GetMoveSpeed();
        foreach (Upgrade upg in upgrades.GetComponents<Upgrade>())
        {
            System.Type upgrade = upg.GetType();
            Instance.gameObject.AddComponent(upgrade);
        }
    }

    public void LoadStats()
    {
        playerStats.health = savedHealth;
        playerStats.maxHealth = savedMaxHealth;
        playerStats.baseMoveSpeed = savedMovespeed;
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

    private void WipeUpgrades()
    {
        foreach (Upgrade upg in gameObject.GetComponents<Upgrade>())
        {
            Destroy(upg);
        }
    }
}
