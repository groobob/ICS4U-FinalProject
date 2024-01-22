using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RogueHeart : BaseStatUpgrade
{
    public void Start()
    {
        base.Init();
        damageBoost = 1;
        weaponRangeBoost = -1.5f;
        upgradeName = "Rogueheart";
        description = "You strike quick and deadly. Trade attack range for damage and passive speed.";
    }
}
