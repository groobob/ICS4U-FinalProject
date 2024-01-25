/*
 * Script for Nimble Cloud. Gives speed.
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedBoost : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        speedBoost = 1.3f;
        upgradeName = "Nimble Cloud";
        description = "Light on your feet, you feel like a cloud, you gain " + speedBoost + " passive movespeed.";
    }
}
