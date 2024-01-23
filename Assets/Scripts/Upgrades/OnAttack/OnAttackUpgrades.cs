/*
 * Superclass to all OnAttackUpgrades. Contains an abstract method to make sure they all have an attack effect.
 * 
 * @author Evan
 * @version January 21
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnAttackUpgrades : Upgrade
{
    [SerializeField] public int attackCount = 1; // how many attacks does this effect occur
    protected PlayerController _playerControl;
    protected PlayerStats _playerStats;

    protected void Init()
    {
        classification = "On attack";
        _playerStats = PlayerManager.Instance._playerStats;
        _playerControl = PlayerManager.Instance._playerControl;
    }


    /**
     * Method representing the effect of the attack.
     */
    public abstract void attack();

    /**
     * Checks if the player's attack count matches the required amount of attacks to activate
     */
    public void upgradeAttack()
    {
        if (_playerControl != null && _playerControl.numOfAttacks % attackCount == 0)
        {
            attack();
            //Debug.Log("Upgrade attack");
        }
    }
}
