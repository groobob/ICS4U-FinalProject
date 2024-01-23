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
        ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 18, 6);

        SoundManager.Instance.PlayAudio(14);
    }

    public void Start()
    {
        base.Init();
        //damageBoost = -1;
        upgradeName = "Magic Path: Grand Wizard";
        description = "Harness the power of magic. Each attack fires projectiles.";
    }
}
