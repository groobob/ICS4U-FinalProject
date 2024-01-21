using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Singleton
    public DataManager Instance;

    // Stats to store
    public enum stats{kills, wins, deaths, besttime, timeplayed}
    float[] storedStats = new float[5];

    void Awake()
    {
        Instance = this;
        Debug.Log("yippee");
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
        WriteToDataFile(storedStats[0] + "\n" + storedStats[1] + "\n" + storedStats[2] + "\n" + storedStats[3] + "\n" + storedStats[4]);
    }

    private void WriteToDataFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Data.txt");
        writer.Write(str);
        writer.Close();
    }

    private void WriteToShopFile(string str)
    {
        StreamWriter writer = new StreamWriter("Assets/Resources/Shop.txt");
        writer.Write(str);
        writer.Close();
    }
}
