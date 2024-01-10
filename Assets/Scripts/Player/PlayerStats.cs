using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{
    public PlayerStats(int HP) : base(HP)
    {

    }
    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }
}
