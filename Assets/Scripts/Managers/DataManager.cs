/*
 * This class manages and stores game data such as kills, wins, deaths, and playtime.
 * 
 * @author Richard
 * @version January 23
 */


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;


public class DataManager : MonoBehaviour
{
    // Singleton
    public static DataManager Instance;

    // Stats to store
    public enum stats{kills, wins, deaths, besttime, timeplayed}
    private float[] storedStats = new float[5];

    /**
     * Awake is called when the script instance is being loaded.
     * It sets the instance to this object, loads data, and starts incrementing time played.
     */
    private void Awake()
    {
        Instance = this;
        LoadData();
        InvokeRepeating("IncrementTimePlayed", 0f, 1f);
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
        switch(type)
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
    }
    /**
     * Sets the specified statistic in the storedStats array to the given value.
     * @param type The type of statistic to set.
     * @param value The value to set for the specified statistic.
     */
    public void SetData(stats type, float time)
    {
        switch(type)
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
    }

    private void OnApplicationQuit()
    {
        WriteToDataFile(storedStats[0] + "|" + storedStats[1] + "|" + storedStats[2] + "|" + storedStats[3] + "|" + Mathf.RoundToInt(storedStats[4]));
    }

    private void WriteToDataFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Data.txt");
        writer.Write(str);
        writer.Close();
    }

    // for shop later maybe
    private void WriteToShopFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Shop.txt");
        writer.Write(str);
        writer.Close();
    }
}
