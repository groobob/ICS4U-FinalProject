/*
 * Superclass to all Melee Weapons.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapons : Weapons
{
    protected float attackWidth;
    protected float attackLength;
    protected float knockbackStrength;

    protected float stunDuration; // should be longer than reloadspeed
    protected int combo;
    protected int comboMax;
    protected float comboResetTime;
    protected float endlagDuration;

    private float comboTimeGiven = 2.5f; // time given to use your next combo attack

    protected Transform attackPoint; // Position where the attack hitbox is.


    /**
     * Constructor for the MeleeWeapons class, initializing the properties of the melee weapon.
     * @param damage The damage inflicted by the melee weapon.
     * @param reloadTime The time required for the weapon to reload after an attack.
     * @param weaponDisplacement The displacement of the weapon from the player's position.
     * @param weaponAngle The angle at which the weapon is positioned relative to the player.
     * @param attackWidth The width of the melee attack area.
     * @param attackLength The length of the melee attack area.
     * @param knockbackStrength The strength of the knockback effect caused by the weapon.
     * @param stunDuration The duration of the stun effect caused by the weapon.
     * @param comboMax The maximum combo count achievable with the weapon.
     * @param endlagDuration The duration of the endlag after an attack.
     * @param player The player controller associated with the weapon.
     */
    public MeleeWeapons(int damage, float reloadTime, float weaponDisplacement, float weaponAngle, float attackWidth, float attackLength, float knockbackStrength, float stunDuration, int comboMax, float endlagDuration, PlayerController player) : base(damage, reloadTime, weaponDisplacement, weaponAngle, player)
    {
        
        this.attackWidth = attackWidth;
        this.attackLength = attackLength;
        this.knockbackStrength = knockbackStrength;
        this.stunDuration = stunDuration;
        this.comboMax = comboMax;
        this.endlagDuration = endlagDuration;
    }

    public void Start()
    {
        combo = 0;
    }

    /**
     * Method for the primary attack of the player.
     */
    public override void Attack()
    {
        SoundManager.Instance.PlayAudio(0);

        _playerStats.EndlagEntity(endlagDuration);
        if (comboResetTime < Time.time) // if you have waited too long to do your next atack
        {
            combo = 0;
            //Debug.Log("Combo timer reset");
        }
        combo++;
        //Debug.Log(combo);
        comboResetTime = Time.time + comboTimeGiven;
        //Debug.Log(_playerStats.bonusRange);
        Collider2D[] hitBox = Physics2D.OverlapBoxAll(_player.GetRealWeaponPosition(), new Vector2(attackLength + _playerStats.bonusRange, attackWidth), _player.GetRealWeaponAngle().eulerAngles.z);
        foreach (Collider2D c in hitBox)
        {
            Enemy enemy = c.gameObject.GetComponent<Enemy>();

            int tempoBoost = 0;
            if (_playerStats.tempo > 50)
            {
                tempoBoost = 1;
            }

            if (enemy && enemy.TakeDamage(_playerStats.bonusDamage + _playerStats.tempDmgBoost + damage + tempoBoost))
            {
                Debug.Log((1 + (int)(_playerStats.tempo / 100)));
                SoundManager.Instance.PlayAudio(6);
                // Things to do on hit
                if (enemy.GetHealth() <= 0)
                {
                    OnKillEffects(enemy);
                }
                OnHitEffects(enemy);
                enemy.StunEntity(stunDuration);
                if (combo == comboMax)
                {
                    enemy.GiveKnockBack(_player.gameObject, knockbackStrength * 5, 0.1f);
                }
                else
                {
                    enemy.GiveKnockBack(_player.gameObject, knockbackStrength, 0.1f);
                }
                //OnHitEffects();
                //enemy.GetComponent<Enemy>().TakeDamage(damage * (1 + (int) (_playerStats.tempo)/100));
                //enemy.GetComponent<Enemy>().GiveKnockBack(_player.gameObject, knockbackStrength, 0.1f);
            }

            if (c.gameObject.GetComponent<Projectile>() && c.gameObject.tag == "EnemyProjectile") // Destroy projectiles
            {
                Destroy(c.gameObject);
            }
        }

        if (combo == comboMax)
        {
            _playerStats.EndlagEntity(1.25f);
            combo = 0;

            Destroy(Instantiate(PlayerManager.Instance.animations[6], _playerController.GetRealWeaponPosition(), _playerController.GetRealWeaponAngle()), 0.55f);
            //Debug.Log("Combo max");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_player.GetRealWeaponPosition(), new Vector2(attackLength, attackWidth));
        //NOTE I CANT ROTATE THIS BRO, SO LIKE GOODLUCK ITS ONLY RLLY ACCURATE WHEN POINTED DIRECTLY RIGHT.
    }
}
