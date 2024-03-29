/*
 * Script for secondary projectile. Does damage when touching enemies. Pierces multiple foes
 * 
 * @author Evan
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SideGunProjectile : Projectile
{

    private List<Enemy> hitEnemies;
    [SerializeField] private float tempoBurstStun;
    [SerializeField] private float knockbakStrength;
    [SerializeField] private float knockbackDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        //Debug.Log(enemy);
        if (enemy)
        {
            bool ignore = false;
            foreach (Enemy check in hitEnemies) // doesn't hit multiple enemies
            {
                Debug.Log(ignore);
                //Debug.Log(check);
                if (check == enemy)
                {
                    ignore = true;
                    Debug.Log("true");
                    break;

                }
            }
            if (!ignore)
            {
                hitEnemies.Add(enemy);
                enemy.TakeDamage(damage + PlayerManager.Instance._playerStats.bonusDamage + PlayerManager.Instance._playerStats.tempDmgBoost);
                enemy.StunEntity(tempoBurstStun);
                enemy.GiveKnockBack(gameObject, knockbakStrength, knockbackDuration);
            }
        }
    }

    private void Start()
    {
        hitEnemies = new List<Enemy>();
        Destroy(gameObject, 0.3f);
    }
}
