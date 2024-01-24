/*
 * Script for Projectile Shooter. Fire projectiles at enemies.
 * 
 * @author Evan
 * @version January 21
 */

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
        ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 18, 6, PlayerManager.Instance._playerControl.GetRealWeaponAngle());

        SoundManager.Instance.PlayAudio(14);
    }

    public void Start()
    {
        base.Init();
        damageBoost = -1;
        upgradeName = "Grand Wizard";
        description = "Harness the power of magic. Each attack fires projectiles.";
    }
}
