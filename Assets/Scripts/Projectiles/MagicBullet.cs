/*
 * Script for player's magic projectile. Does damage when touching enemies
 * 
 * @author Evan
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : Projectile
{
    public Color[] colors; // Array of colors to choose from
    private SpriteRenderer spriteRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        RandomizeGameObjectColor();
    }

    private void RandomizeGameObjectColor()
    {
        if (colors.Length > 0)
        {
            int randomIndex = Random.Range(0, colors.Length);
            spriteRenderer.color = colors[randomIndex];
        }
    }

}
