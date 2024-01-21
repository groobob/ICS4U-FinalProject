using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : Projectile
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats _playerStats = collision.gameObject.transform.parent.gameObject.GetComponent<PlayerStats>();
        if (_playerStats)
        {
            _playerStats.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
