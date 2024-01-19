using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedBoost : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        speedBoost = 1.5f;
        upgradeName = "Nimble Cloud";
        description = "Light on your feet, you feel like a cloud, you gain +1.5 passive movespeed.";
    }
}
