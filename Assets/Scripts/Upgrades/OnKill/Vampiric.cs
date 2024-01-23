/*
 * Script for Vampiric. After killing 7 enemies, heal HP. 
 * 
 * @author Evan
 * @version January 22
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vampiric : OnKillUpgrades
{
    private int enemiesCount = 7;
    private int current;

    public void Start()
    {
        current = 0;
        base.Init();
        upgradeName = "Vampiric";
        description = "Heal every " + enemiesCount + " enemies killed.";
    }

    public override void attackEffect()
    {
        Debug.Log("Vamp");
        if (current < enemiesCount)
        {
            current++;
        }
        else
        {
            current = 0;
            SoundManager.Instance.PlayAudio(9);
            _playerStats.HealDamage(1);
        }
    }
}
