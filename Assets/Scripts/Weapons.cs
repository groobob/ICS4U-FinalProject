using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    protected int damage; // damage dealt
    protected float reloadTime; // time between attacks
    //private bool meleeWeapon; // shoots projectiles or no?

    //private float attackRange;
    //visuals
    protected float weaponDisplacement; // how far from player should it be held
    protected float weaponAngle; // what angle offset should the weapon appear to have
    protected PlayerController _player;

    public Weapons(int damage, float reloadTime, float weaponDisplacement, float weaponAngle, PlayerController player)
    {
        this.damage = damage;
        this.reloadTime = reloadTime;
        this.weaponDisplacement = weaponDisplacement;
        this.weaponAngle = weaponAngle;
        _player = player;
    }

    public abstract void Attack();

    public float getReloadTime()
    {
        return reloadTime;
    }

    public void SetPlayer(PlayerController plr)
    {
        _player = plr;
        Debug.Log(damage + " " + reloadTime + " " + _player);
    }
    public float GetWeaponDisplacement()
    {
        return weaponDisplacement;
    }

    public float GetWeaponAngle()
    {
        return weaponAngle;
    }
}
