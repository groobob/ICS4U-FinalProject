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

    void Awake()
    {
        Instance = this;
        LoadData();
        InvokeRepeating("IncrementTimePlayed", 0f, 1f);
    }

    private void IncrementTimePlayed()
    {
        storedStats[4]++;
    }

    public void ResetDataToDefault()
    {
        for (int i = 0; i < storedStats.Length; i++)
        {
            storedStats[i] = 0;
        }
        WriteToDataFile("0|0|0|0|0");
    }

    public void LoadData()
    {
        TextAsset text = Resources.Load<TextAsset>("Data");
        string[] loadedData = text.text.Split("|");

        for (int i = 0; i < loadedData.Length; i++)
        {
            storedStats[i] = int.Parse(loadedData[i]);
        }
    }

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
