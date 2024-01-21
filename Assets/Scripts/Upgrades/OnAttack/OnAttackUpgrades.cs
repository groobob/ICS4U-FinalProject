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
        _playerControl = transform.parent.gameObject.GetComponent<PlayerController>();
        _playerStats = transform.parent.gameObject.GetComponent<PlayerStats>();
    }

    public abstract void attack();

    public void upgradeAttack()
    {
        if (_playerControl != null && _playerControl.numOfAttacks % attackCount == 0)
        {
            attack();
            //Debug.Log("Upgrade attack");
        }
    }
}
