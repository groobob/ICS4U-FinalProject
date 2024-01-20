/*
 * Superclass to all Enemies.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    public Transform target;
    public Transform sprite;
    [Header("Enemy Values")]
    [SerializeField] protected float detectionRadiusSquared;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float meleeRange;
    private float nextAttackTime;


    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }

    protected abstract void Attack();

    protected void AttackCheck()
    {
        if (nextAttackTime < Time.time)
        {
            Attack();
            nextAttackTime = Time.time + attackSpeed;
        }
    }
}
