using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bloodmoon : OnKillUpgrades
{
    public void Start()
    {
        base.Init();
        speedBoost = 0.6f;
        upgradeName = "Bloodmoon";
        description = "Gain " + speedBoost + " movespeed, have chance of healing each time you defeat an enemy, scaling with bonus movespeed.";
    }

    public override void attackEffect()
    {
        Debug.Log("BloodMoon");
        float baseSpeed = _playerStats.GetMoveSpeed() - PlayerManager.Instance.addedMovespeed;
        int interval2 = 21/ (int)Mathf.Ceil((_playerStats.GetMoveSpeed() * _playerController.ApplySpeedModsPlayer() - baseSpeed));
        if (Random.Range(1, interval2) == 1)
        {
            _playerStats.HealDamage(1);
        }
    }
}
