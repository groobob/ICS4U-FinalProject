using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy(int HP) : base(HP)
    {
        
    }
    public override void AddEntity()
    {
        throw new System.NotImplementedException();
    }

    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEntity()
    {
        throw new System.NotImplementedException();
    }
}
