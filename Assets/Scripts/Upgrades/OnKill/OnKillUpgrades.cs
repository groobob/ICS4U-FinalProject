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

    public abstract void attackEffect();
}
