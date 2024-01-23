/*
 * Script for giving phantom step secondary upgrade.
 * 
 * @author Evan
 * @version January 22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomStepUpgrade : SecondaryChange
{
    public void Start()
    {
        base.Init();
        upgradeName = "Phantom Step";
        description = "Enter a spectral form where you cannot be damage and gain a speed boost. Costs tempo.";
    }
}
