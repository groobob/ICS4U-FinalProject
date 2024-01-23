/*
 * Script for Tempo Master. Gain more tempo and tempo gain.
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempoMaster : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        tempoGainBoost = 5f;
        tempoMaxBoost = 50f;
        upgradeName = "Tempo Master";
        description = "Your focus increases. You gain additional max tempo and tempo per attack.";
    }
}
