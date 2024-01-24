/*
 * Script for player's magic projectile. Does damage when touching enemies
 * 
 * @author Evan
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(2);
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}
