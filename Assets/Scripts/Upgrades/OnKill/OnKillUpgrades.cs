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
        _playerStats = transform.parent.gameObject.GetComponent<PlayerStats>();
        _playerController = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    public abstract void attackEffect();
}
