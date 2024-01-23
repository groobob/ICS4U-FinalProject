using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Earthquake : Upgrade
{

    private GameObject EQ;
    public void Start()
    {
        EQ = ProjectileManager.Instance.GetProjectileList().ElementAt(5);
        tempoGainBoost = 3f;
        upgradeName = "Circle of Fire";
        description = "Tempo bursting creates an AOE field that damages and slows enemies. Also gives bonus Tempo Gain on basic attacks";
        classification = "Tempo Burst Upgrade";
    }

    public void attackEffect()
    {
        Debug.Log("Circle of Fire");
        Vector3 pos = transform.position;
        pos.z = 0f;
        Instantiate(EQ, pos, Quaternion.identity);
    }
}
