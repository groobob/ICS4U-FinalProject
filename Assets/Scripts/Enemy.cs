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
    public Transform target;
    public Transform sprite;
    public Enemy(int HP) : base(HP)
    {

    }

    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }
}
