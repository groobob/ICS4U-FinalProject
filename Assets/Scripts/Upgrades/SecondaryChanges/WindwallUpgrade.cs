/*
 * Script for giving windwall secondary upgrade.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindwallUpgrade : SecondaryChange
{
    public void Start()
    {
        base.Init();
        upgradeName = "Wind wall";
        description = "Face the winds! Your secondary is now replaced with a wind wall that blocks enemy projectiles.";
    }
}
