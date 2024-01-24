/*
 * This class manages and stores game data such as kills, wins, deaths, and playtime.
 * 
 * @author Richard
 * @version January 23
 */


using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    // Singleton
    public static DataManager Instance;

    // Stats to store
    public enum stats { kills, wins, deaths, besttime, timeplayed }
    public enum upgrade { speed, damage, health }
    private float[] storedStats = new float[5];
    private int[] storedUpgrades = new int[3];

    // Values
    public int money;
    public string upgradesObtained;
    private float speedrunTimer;
    private bool timerOn = false;

    // References
    [Header("References")]
    [SerializeField] private TextMeshProUGUI speedrunTimerText;

    /**
     * Awake is called when the script instance is being loaded.
     * It sets the instance to this object, loads data, and starts incrementing time played.
     */
    private void Awake()
    {
        Instance = this;
        LoadData();
        LoadShop();
        InvokeRepeating("IncrementTimePlayed", 0f, 1f);
    }

    private void Update()
    {
        if (timerOn)
        {
            speedrunTimer += Time.deltaTime;
            if (!(speedrunTimerText == null))
            {
                speedrunTimerText.text = GetSpeedrunTimerTime();
            }
        }
    }

    /*
     * Get the string representation of all the upgrades obtained from upgrade manager
     */
    public void UpdateUpgradesObtainedList()
    {
        upgradesObtained = UpgradeManager.Instance.GetObtainedUpgrades();
    }

    /*
    * Method to run calculations for the timer
    * 
    * @return string - Representation of the current speedrun time
    */
    public string GetSpeedrunTimerTime()
    {
        int currentTime = Mathf.RoundToInt(speedrunTimer);
        int hrs = Mathf.FloorToInt(currentTime / 3600);
        int min = Mathf.FloorToInt(currentTime % 3600 / 60);
        int sec = currentTime % 3600 % 60;
        return hrs + ":" + min + ":" + sec;
    }

    /*
    * Starts the speedrun timer
    */
    public void StartTimer()
    {
        timerOn = true;
    }

    /*
    * Stops the speedrun timer
    */
    public void StopTimer()
    {
        timerOn = false;
    }

    /*
    * Resets the timer
    */
    public void ResetTimer()
    {
        timerOn = false;
        speedrunTimer = 0f;
    }

    // Shop related methods

    /*
     * Increments your balance with a given parameter
     * 
     * @param amount - Amount of money to increment
     */
    public void IncrementMoney(int amount)
    {
        money += amount;
        SaveToFiles();
    }

    /*
     * Gives the information from the upgrades
     * 
     * @return int - The upgrade progression
     */
    public int GetShop(upgrade type)
    {
        switch (type)
        {
            case upgrade.speed:
                return storedUpgrades[0];
            case upgrade.damage:
                return storedUpgrades[1];
            case upgrade.health:
                return storedUpgrades[2];
        }
        return -1;
    }

    /*
     * Increments a certain upgrade
     * 
     * @param type - The type of upgrade to increment
     */
    public void IncrementShop(upgrade type)
    {
        switch(type)
        {
            case upgrade.speed:
                storedUpgrades[0]++;
                break;
            case upgrade.damage:
                storedUpgrades[1]++;
                break;
            case upgrade.health:
                storedUpgrades[2]++;
                break;
        }
        SaveToFiles();
    }

    /**
    * Loads data from the "Shop" resource file and populates the storedUpgrades array.
    */
    public void LoadShop()
    {
        TextAsset text = Resources.Load<TextAsset>("Shop");
        string[] loadedData = text.text.Split("|");

        for (int i = 0; i < loadedData.Length; i++)
        {
            if (i == loadedData.Length - 1)
            {
                money = int.Parse(loadedData[i]);
                break;
            }
            storedUpgrades[i] = int.Parse(loadedData[i]);
        }
    }

    /**
    * Resets all stored shop data to default values and writes to the shop file.
    */
    public void ResetShopToDefault()
    {
        money = 0;
        for (int i = 0; i < storedUpgrades.Length; i++)
        {
            storedUpgrades[i] = 0;
        }
        WriteToShopFile("0|0|0|0");
    }

    private void WriteToShopFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Shop.txt");
        writer.Write(str);
        writer.Close();
    }

    // Data related methods

    public void CompareBestTimes()
    {
        if (speedrunTimer < storedStats[3] || storedStats[3] == 0)
        {
            SetData(stats.besttime, speedrunTimer);
        }
    }

    private void IncrementTimePlayed()
    {
        storedStats[4]++;
    }
    /**
    * Resets all stored data to default values and writes to the data file.
    */
    public void ResetDataToDefault()
    {
        for (int i = 0; i < storedStats.Length; i++)
        {
            storedStats[i] = 0;
        }
        WriteToDataFile("0|0|0|0|0");
    }
    /**
        * Loads data from the "Data" resource file and populates the storedStats array.
        */
    public void LoadData()
    {
        TextAsset text = Resources.Load<TextAsset>("Data");
        string[] loadedData = text.text.Split("|");

        for (int i = 0; i < loadedData.Length; i++)
        {
            storedStats[i] = int.Parse(loadedData[i]);
        }
    }
    /**
    * Gets the specified statistic from the storedStats array.
    * @param type The type of statistic to retrieve.
    * @return The value of the specified statistic.
    */
    public float GetData(stats type)
    {
        switch (type)
        {
            case stats.kills:
                return storedStats[0];
            case stats.wins:
                return storedStats[1];
            case stats.deaths:
                return storedStats[2];
            case stats.besttime:
                return storedStats[3];
            case stats.timeplayed:
                return storedStats[4];
        }
        return -1f;
    }
    /**
        * Increments the specified statistic in the storedStats array.
        * @param type The type of statistic to increment.
        */
    public void IncrementData(stats type)
    {
        switch (type)
        {
            case stats.kills:
                storedStats[0]++;
                break;
            case stats.wins:
                storedStats[1]++;
                break;
            case stats.deaths:
                storedStats[2]++;
                break;
            default:
                break;
        }
        SaveToFiles();
    }
    /**
        * Sets the specified statistic in the storedStats array to the given value.
        * @param type The type of statistic to set.
        * @param value The value to set for the specified statistic.
        */
    public void SetData(stats type, float time)
    {
        switch (type)
        {
            case stats.besttime:
                storedStats[3] = time;
                break;
            case stats.timeplayed:
                storedStats[4] = time;
                break;
            default:
                break;
        }
        SaveToFiles();
    }

    private void OnApplicationQuit()
    {
        SaveToFiles();
    }

    private void SaveToFiles()
    {
        WriteToDataFile(storedStats[0] + "|" + storedStats[1] + "|" + storedStats[2] + "|" + storedStats[3] + "|" + Mathf.RoundToInt(storedStats[4]));
        WriteToShopFile(storedUpgrades[0] + "|" + storedUpgrades[1] + "|" + storedUpgrades[2] + "|" + money);
    }

    private void WriteToDataFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Data.txt");
        writer.Write(str);
        writer.Close();
    }
}
