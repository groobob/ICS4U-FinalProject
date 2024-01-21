using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OwlSlice : OnAttackUpgrades
{
    
    public override void attack()
    {
        _playerStats.SpeedBoost(1.4f, 3f);
        Debug.Log("Owl Slice");
    }

    public void Start()
    {
        attackCount = 3;
        damageBoost = 2;
        base.Init();
        upgradeName = "Owl's Slice";
        description = "Every three attacks or secondary attack, gain a speed boost. Additionally, gain attack damage.";
    }
}
