using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CurseOfAnguish : OnAttackUpgrades
{

    private float AOEsize = 7f;
    private float knockbackStrength = 15f;
    public override void attack()
    {
        if (Random.Range(1, 8) == 1)
        {
            Debug.Log("Curse of Anguish");
            // AOE slash
            Collider2D[] hitBox = Physics2D.OverlapBoxAll(_playerControl.GetRealWeaponPosition(), new Vector2(AOEsize, AOEsize), _playerControl.GetRealWeaponAngle().eulerAngles.z);
            foreach (Collider2D c in hitBox)
            {
                Enemy enemy = c.gameObject.GetComponent<Enemy>();
                if (enemy && enemy.TakeDamage(15))
                {
                    enemy.GiveKnockBack(_playerControl.gameObject, knockbackStrength, 0.1f);
                    enemy.StunEntity(1f);
                    _playerStats.EndlagEntity(0.5f);
                    _playerStats.SpeedBoost(0.3f, 0.5f);
                }

                if (c.gameObject.GetComponent<Projectile>() && c.gameObject.tag == "EnemyProjectile")
                {
                    Destroy(c.gameObject);
                }
            }
            if (Random.Range(1,3) == 1)
            {
                _playerStats.TakeDamage(1);
            }
        }
        
    }

    public void Start()
    {
        base.Init();
        upgradeName = "Curse of Anguish";
        description = "Sorrow for strength. Randomly when attacking, summon a flurry of slashes. Proccing this effect has a chance to deal damage to the user.";
    }
}
