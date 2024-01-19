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

    protected Transform attackPoint; // Position where the attack hitbox is.


    //NOTE: BASE KEYWORD IS BASICALLY CALLING THE SUPER CONSTRUCTOR
    /**
     * Constructor for MeleeWeapons.
     */
    public MeleeWeapons(int damage, float reloadTime, float weaponDisplacement, float weaponAngle, float attackWidth, float attackLength, PlayerController player) : base(damage, reloadTime, weaponDisplacement, weaponAngle, player)
    {
        
        this.attackWidth = attackWidth;
        this.attackLength = attackLength;
    }
    /**
     * Method for the primary attack of the player.
     */
    public override void Attack()
    {
        Collider2D[] hitBox = Physics2D.OverlapBoxAll(_player.GetRealWeaponPosition(), new Vector2(attackLength, attackWidth), _player.GetRealWeaponAngle().eulerAngles.z);
        foreach (Collider2D c in hitBox)
        {
            Enemy enemy = c.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                OnHitEffects();
                enemy.GetComponent<Enemy>().TakeDamage(damage * (1 + (int) (_playerStats.tempo)/100));
                
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_player.GetRealWeaponPosition(), new Vector2(attackLength, attackWidth));
        //NOTE I CANT ROTATE THIS BRO, SO LIKE GOODLUCK ITS ONLY RLLY ACCURATE WHEN POINTED DIRECTLY RIGHT.
    }
}
