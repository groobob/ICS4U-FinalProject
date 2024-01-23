/*
 * Script for healthpack. Gives 1 HP.
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        healthBoost = 1;
        upgradeName = "Health Pack";
        description = "Gain 1 HP. Simple.";
    }
}
