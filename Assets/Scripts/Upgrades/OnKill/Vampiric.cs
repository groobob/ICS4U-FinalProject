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

    public override void attackEffect(Enemy e)
    {
        Debug.Log("Vamp");
        if (current < enemiesCount)
        {
            current++;
        }
        else
        {
            Destroy(Instantiate(PlayerManager.Instance.animations[5], e.transform.position, Quaternion.identity), 0.533f);
            current = 0;
            SoundManager.Instance.PlayAudio(9);
            _playerStats.HealDamage(1);
        }
    }
}
