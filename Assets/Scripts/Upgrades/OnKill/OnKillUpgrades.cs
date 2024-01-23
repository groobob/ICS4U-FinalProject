/*
 * Superclass for all OnKillUpgrades. These procc when an enemy is killed.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnKillUpgrades : Upgrade
{
    protected PlayerStats _playerStats;
    protected PlayerController _playerController;

    protected void Init()
    {
        classification = "On kill";
        _playerStats = PlayerManager.Instance._playerStats;
        _playerController = PlayerManager.Instance._playerControl;
    }

    /**
     * Effect to procc when enemy is killed.
     */
    public abstract void attackEffect();
}
