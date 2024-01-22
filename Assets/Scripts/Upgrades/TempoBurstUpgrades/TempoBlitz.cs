using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempoBlitz : Upgrade
{
    public void Start()
    {
        tempoGainBoost = 5f;
        classification = "Tempo Burst Upgrade";
        upgradeName = "Tempo Blitz";
        description = "Eliminating a target with Tempo Burst resets the cooldown of Tempo Burst. Additionally, gain more tempo per attack";
    }
}
