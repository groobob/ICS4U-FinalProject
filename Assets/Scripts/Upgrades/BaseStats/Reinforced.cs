using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforced : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        healthBoost = 2;
        upgradeName = "Reinforced";
        description = "Gain 2 Passive HP.";
    }
}
