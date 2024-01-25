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
        string[] data = FindObjectOfType<Info>().GetData().Split("|");
        upgradesCollectedText.text = data[0];
        timeSpentText.text = data[1];
    }
}
