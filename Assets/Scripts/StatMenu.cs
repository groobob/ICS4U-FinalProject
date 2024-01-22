using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI winsText;
    [SerializeField] TextMeshProUGUI deathsText;
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI timePlayedText;

    private void Awake()
    {
        UpdateData();
    }

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

    public void ResetData()
    {
        DataManager.Instance.ResetDataToDefault();
        UpdateData();
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
