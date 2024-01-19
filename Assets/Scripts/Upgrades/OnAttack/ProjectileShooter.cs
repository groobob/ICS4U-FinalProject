using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : OnAttackUpgrades
{
    public override void attack()
    {
        Debug.Log("Richard add your projectile here");
    }

    public void Start()
    {
        base.Init();
        upgradeName = "Projectile Shooter";
        description = "Every three attacks, fire a projectile";
    }
}
