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
    private int damage = 4;
    private float stunDuration = 1.4f;
    private float previousTime;

    private bool reset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy)
        {
            bool ignore = false;
            foreach (Enemy check in hitEnemies)
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
                enemy.TakeDamage(damage);
                enemy.StunEntity(stunDuration);
            }
        }
    }

    private void Update()
    {
        if (previousTime - Time.time >= duration/2 && !reset)// does two ticks of damage
        {
            reset = true;
            hitEnemies = new List<Enemy>(); // resets the list of already hit enemies
        }
    }

    private void Start()
    {
        reset = false;
        previousTime = Time.time;
        hitEnemies = new List<Enemy>();
        Destroy(gameObject, duration);
    }
}
