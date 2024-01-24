/*
 * Script for Reinforced. Gives 2 HP.
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforced : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        healthBoost = 3;
        upgradeName = "Reinforced";
        description = "Gain 2 Passive HP.";
    }
}
