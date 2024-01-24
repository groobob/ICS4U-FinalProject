/*
 * Class to manage and compile the information that is shown at the end of runs
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradesCollectedText;
    [SerializeField] private TextMeshProUGUI timeSpentText;

    private void Start()
    {
        upgradesCollectedText.text = DataManager.Instance.upgradesObtained;
        timeSpentText.text = DataManager.Instance.GetSpeedrunTimerTime();
        UpgradeManager.Instance.Reset();
    }
}
