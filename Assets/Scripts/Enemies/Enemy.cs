/*
 * Superclass to all Enemies.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected float detectionRadius;
    public Enemy(int HP, float speed, float detectionRadius) : base(HP, speed)
    {
        this.detectionRadius = detectionRadius;
    }

    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }
}
