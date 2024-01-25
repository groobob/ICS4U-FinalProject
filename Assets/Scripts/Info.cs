/*
 * Class to bring data across scenes, specifically the time and upgrades
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public static Info Instance;
    public string text1;
    public string text2;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*
     * Sets the data
     * 
     * @param upgrade - The upgrades obtained
     * @param time - Time elapsed in run
     */
    public void SetData(string upgrade, string time)
    {
        text1 = upgrade; text2 = time;
    }

    /*
     * Returns the data
     * 
     * @return string - String representation of the data
     */
    public string GetData()
    {
        Destroy(gameObject, 0.5f);
        return text1 + "|" + text2;
    }
}
