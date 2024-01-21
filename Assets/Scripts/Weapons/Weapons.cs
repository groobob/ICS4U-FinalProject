using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Weapons(int damage, float reloadTime, float weaponDisplacement, float weaponAngle, PlayerController player)
    {
        this.damage = damage;
        this.reloadTime = reloadTime;
        this.weaponDisplacement = weaponDisplacement;
        this.weaponAngle = weaponAngle;
        _player = player;
    }

    public abstract void Attack();

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public void SetPlayer(PlayerController plr)
    {
        _player = plr;
        _playerStats = plr.gameObject.GetComponent<PlayerStats>();
        //Debug.Log(damage + " " + reloadTime + " " + _player);
    }
    public float GetWeaponDisplacement()
    {
        return weaponDisplacement;
    }

    public int GetWeaponDamage()
    {
        return damage;
    }

    public float GetWeaponAngle()
    {
        return weaponAngle;
    }

    protected void OnHitEffects()
    {
        _playerStats.AddTempo();
    }
}
