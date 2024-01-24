/*
 * Superclass to all primary weapons
 * 
 * @author Evan
 * @version January 09
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataManager;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Weapons : MonoBehaviour
{
    protected int damage; // damage dealt
    protected float reloadTime; // time between attacks

    //private float attackRange;
    //visuals
    protected float weaponDisplacement; // how far from player should it be held
    protected float weaponAngle; // what angle offset should the weapon appear to have
    protected PlayerController _player;
    protected PlayerStats _playerStats;

    /**
 * Constructor for the Weapons class, initializing the properties of the weapon.
 * @param damage The damage inflicted by the weapon.
 * @param reloadTime The time required for the weapon to reload after an attack.
 * @param weaponDisplacement The displacement of the weapon from the player's position.
 * @param weaponAngle The angle at which the weapon is positioned relative to the player.
 * @param player The player controller associated with the weapon.
 */
    public Weapons(int damage, float reloadTime, float weaponDisplacement, float weaponAngle, PlayerController player)
    {
        this.damage = damage;
        this.reloadTime = reloadTime;
        this.weaponDisplacement = weaponDisplacement;
        this.weaponAngle = weaponAngle;
        _player = player;
    }
    /**
     * Abstract method for performing an attack with the weapon.
     */
    public abstract void Attack();

    /**
     * Retrieves the reload time of the weapon.
     * @return float
     */
    public float GetReloadTime()
    {
        return reloadTime;
    }
    /**
     * Sets the player associated with the weapon.
     * @param plr The player controller to be associated with the weapon.
     */
    public void SetPlayer(PlayerController plr)
    {
        _player = plr;
        _playerStats = plr.gameObject.GetComponent<PlayerStats>();
        //Debug.Log(damage + " " + reloadTime + " " + _player);
    }
    /**
     * Retrieves the displacement of the weapon.
     * @return float
     */
    public float GetWeaponDisplacement()
    {
        return weaponDisplacement;
    }

    /**
     * Retrieves the damage inflicted by the weapon.
     * @return The damage inflicted by the weapon.
     */

    public int GetWeaponDamage()
    {
        return damage;
    }

    /**
     * Retrieves the angle at which the weapon is positioned.
     * @return float
     */
    public float GetWeaponAngle()
    {
        return weaponAngle;
    }

    /**
     * Handles the effects when the weapon hits an enemy.
     * @param enemy The enemy affected by the weapon hit.
     */
    protected void OnHitEffects(Enemy enemy)
    {
        _playerStats.AddTempo();

        foreach (OnHitUpgrades upgrade in PlayerManager.Instance.GetUpgradesPart().GetComponents<OnHitUpgrades>())
        {
            upgrade.attackEffect();
        }
    }

    /**
     * Handles the effects when the weapon kills an enemy.
     * @param enemy The enemy killed by the weapon.
     */
    protected void OnKillEffects(Enemy enemy)
    {
        foreach (Upgrade upg in PlayerManager.Instance.GetUpgradesList())
        {
            OnKillUpgrades hitUpgrade = upg as OnKillUpgrades;
            if (hitUpgrade != null)
            {
                hitUpgrade.attackEffect();
            }
        }
    }
}
