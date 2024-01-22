using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OwlClaw : OnHitUpgrades
{
    public void Start()
    {
        base.Init();
        speedBoost = 0.5f;
        upgradeName = "Owl's Claw";
        description = "Gain " + speedBoost + " movespeed, deal bonus damage for each point of speed above base movespeed. However, lose damage when you are slowed.";
    }

    private void Update()
    {
        float baseSpeed = _playerStats.GetMoveSpeed() - PlayerManager.Instance.GetAddedMoveSpeed();
        tempDmg = (int) Mathf.Ceil((_playerStats.GetMoveSpeed() * _playerController.ApplySpeedModsPlayer() - baseSpeed));
        if (_playerController.GetWeapon().GetWeaponDamage() + tempDmg <= 1)
        {
            tempDmg = _playerController.GetWeapon().GetWeaponDamage() - 1;
        }
        Debug.Log(tempDmg + " damage boost"); 
    }
    public override void attackEffect()
    {
        throw new System.NotImplementedException();
    }
}
