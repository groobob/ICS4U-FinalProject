using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitUpgrades : Upgrade
{
    //[SerializeField] public int attackCount = 1; // how many attacks does this effect occur
    protected PlayerStats _playerStats;
    protected PlayerController _playerController;

    protected void Init()
    {
        classification = "On hit";
        _playerStats = PlayerManager.Instance._playerStats;
        _playerController = PlayerManager.Instance._playerControl;
    }

    public abstract void attackEffect();
}
