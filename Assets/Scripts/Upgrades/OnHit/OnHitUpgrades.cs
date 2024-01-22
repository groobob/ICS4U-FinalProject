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
        _playerStats = transform.parent.gameObject.GetComponent<PlayerStats>();
        _playerController = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    public abstract void attackEffect();
}
