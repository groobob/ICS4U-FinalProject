using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : OnAttackUpgrades
{
    Vector3 mousePos;
    Vector3 mousePlayerVector;
    public override void attack()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mousePlayerVector = (mousePos - transform.position).normalized;
        ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 8, 0);
    }

    public void Start()
    {
        base.Init();
        upgradeName = "Projectile Shooter";
        description = "Every three attacks, fire a projectile";
    }
}
