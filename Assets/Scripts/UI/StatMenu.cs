/*
 * Script for managing stats that are to be saved and reloaded.
 * 
 * @author Richard
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private TextMeshProUGUI deathsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI timePlayedText;

    private void Awake()
    {
        UpdateData();
    }

    /**
     * Updates the data displayed in stats menu.
     */
    public void UpdateData()
    {
        winsText.text = "Wins: " + DataManager.Instance.GetData(DataManager.stats.wins);
        deathsText.text = "Deaths: " + DataManager.Instance.GetData(DataManager.stats.deaths);
        killsText.text = "Kills: " + DataManager.Instance.GetData(DataManager.stats.kills);
        bestTimeText.text = DataManager.Instance.GetData(DataManager.stats.besttime) == 0 ? "na" : "" + DataManager.Instance.GetData(DataManager.stats.besttime);
        timePlayedText.text = Mathf.RoundToInt(DataManager.Instance.GetData(DataManager.stats.timeplayed)) + "sec";
    }

    private void Update()
    {
        timePlayedText.text = Mathf.RoundToInt(DataManager.Instance.GetData(DataManager.stats.timeplayed)) + "sec";
    }
    /**
     * Resets data in stats menu.
     */
    public void ResetData()
    {
        DataManager.Instance.ResetDataToDefault();
        Invoke("UpdateData", 0.1f);
    }
    /**
     * Closes stats menu.
     */
    public void Close()
    {
        Destroy(gameObject);
    }
}
