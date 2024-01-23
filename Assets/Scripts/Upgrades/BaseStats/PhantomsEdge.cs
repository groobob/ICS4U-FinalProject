/*
 * Script for Phantom's Edge. Gives bonus range.
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomsEdge : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        weaponRangeBoost = 3f;
        upgradeName = "Phantom's Edge";
        description = "Your blade is longer than it appears, gain 3 attack length";
    }
}
