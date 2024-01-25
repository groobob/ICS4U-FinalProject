/*
 * Script for causing FireColumn/DarkBolt damage
 * 
 * @author Evan
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageBlock : MonoBehaviour
{
    private float duration = 3f;
    private List<Enemy> hitEnemies;
    private int damage = 8;
    private float stunDuration = 1.4f;

    private void TickDamage()
    {
        SoundManager.Instance.PlayAudio(8);
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x);
        foreach (Collider2D c in hitBox)
        {
            Enemy enemy = c.gameObject.GetComponent<Enemy>();
            if (enemy && enemy.TakeDamage(damage))
            {
                enemy.GiveKnockBack(gameObject, 2f, 0.1f);
                enemy.StunEntity(stunDuration);
            }

            if (c.gameObject.GetComponent<Projectile>() && c.gameObject.tag == "EnemyProjectile")
            {
                Destroy(c.gameObject);
            }
        }
    }

    private void Start()
    {
        hitEnemies = new List<Enemy>();
        TickDamage();
        Destroy(gameObject, duration);
    }
}
