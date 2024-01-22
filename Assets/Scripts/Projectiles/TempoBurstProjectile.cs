using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TempoBurstProjectile : Projectile
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
                if (enemy.health - damage <= 0 && PlayerManager.Instance.GetUpgradesPart().GetComponent<TempoBlitz>())
                {
                    Debug.Log("TempoBlitz");
                    PlayerManager.Instance.ResetTempoBurstCD();
                }

                hitEnemies.Add(enemy);
                enemy.TakeDamage(damage);
                enemy.StunEntity(tempoBurstStun);
                enemy.GiveKnockBack(gameObject, knockbakStrength, knockbackDuration);
            }
        }
    }

    private new void Start()
    {
        hitEnemies = new List<Enemy>();
        Destroy(gameObject, 0.3f);
    }
}