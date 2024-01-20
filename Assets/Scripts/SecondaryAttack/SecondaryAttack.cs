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

    public SecondaryAttack(int damage, float reloadTime, PlayerController player)
    {
        this.damage = damage;
        this.reloadTime = reloadTime;
        _player = player;
    }

    public void SetPlayer(PlayerController plr)
    {
        _player = plr;
        _playerStats = plr.gameObject.GetComponent<PlayerStats>();
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public abstract void Attack();
}
