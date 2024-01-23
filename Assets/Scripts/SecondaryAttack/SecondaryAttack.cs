/*
 * Superclass for all secondary attacks.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class SecondaryAttack : MonoBehaviour
{
    protected int damage; // damage dealt
    protected float reloadTime; // time between attacks

    protected PlayerController _player;
    protected PlayerStats _playerStats;

    /**
     * Main Constructor for Secondary Attack
     * @param damage Amount of damage the attack should deal
     * @param reloadTime Time it takes to reload
     * @param player The player controller of the current player
     */
    public SecondaryAttack(int damage, float reloadTime, PlayerController player)
    {
        this.damage = damage;
        this.reloadTime = reloadTime;
        _player = player;
    }
    /**
     * Sets the player variables in the script.
     * @param plr Player controller of the player.
     */
    public void SetPlayer(PlayerController plr)
    {
        _player = plr;
        _playerStats = plr.gameObject.GetComponent<PlayerStats>();
    }
    /**
     * Returns the reload time of the secondary attack
     * @return float
     */
    public float GetReloadTime()
    {
        return reloadTime;
    }
    /**
     * Represents the activating effect of the secondary attack.
     */
    public abstract void Attack();
}
