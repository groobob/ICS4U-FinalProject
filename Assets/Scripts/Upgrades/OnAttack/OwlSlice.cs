/*
 * Script for Owl Slice. Gain a speed boost every three attacks or secondary. 
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OwlSlice : OnAttackUpgrades
{
    
    public override void attack()
    {
        _playerStats.SpeedBoost(1.15f, 1f);
        Debug.Log("Owl Slice");
    }

    public void Start()
    {
        attackCount = 3;
        damageBoost = 1;
        base.Init();
        upgradeName = "Owl's Slice";
        description = "Every three attacks or secondary attack, gain a speed boost. Additionally, gain attack damage.";
    }
}
