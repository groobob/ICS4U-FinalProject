/*
 * Script for Haunted Pressence. Summon an AOE attack every four hits. 
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HauntedPresence : OnHitUpgrades
{
    private int hitCount = 3;
    private int current;

    private float AOEsize = 6f;

    public void Start()
    {
        current = 0;
        base.Init();
        upgradeName = "Haunted Presence";
        description = "On every 4th hit attack, summon a ghostly follow up strike.";
    }

    public override void attackEffect()
    {
        if (current < hitCount)
        {
            current++;
        }
        else
        {
            current = 0;
            Debug.Log("Haunted Presence");
            Collider2D[] hitBox = Physics2D.OverlapBoxAll(_playerController.GetRealWeaponPosition(), new Vector2(AOEsize, AOEsize), _playerController.GetRealWeaponAngle().eulerAngles.z);
            Destroy(Instantiate(PlayerManager.Instance.animations[4], _playerController.GetRealWeaponPosition(), _playerController.GetRealWeaponAngle()), 0.55f);
            foreach (Collider2D c in hitBox) // AOE hitbox
            {
                Enemy enemy = c.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage(2);
                }

                if (c.gameObject.GetComponent<Projectile>() && c.gameObject.tag == "EnemyProjectile")
                {
                    Destroy(c.gameObject);
                }
            }
        }
    }
}
