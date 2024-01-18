using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Entity
{

    // Base Stats
    private static int baseHP = 7;
    private static float baseMS = 5f;
    /**
     * Main constructor for PlayerStats.
     * @param HP Max Health for Player.
     * @param speed Base speed for Player.
     */
    public PlayerStats() : base(baseHP, baseMS)
    {
    }

    private void Start()
    {
        // Does things if a new player needs to be made
        if (PlayerManager.Instance.isNew)
        {
            PlayerManager.Instance.isNew = false;
            setBaseStats(baseHP, baseMS);
        }
    }

    public override void DeathEvent()
    {
        throw new System.NotImplementedException();
    }

    public void AddUpgrades(System.Type upgrade)
    {
        //Upgrade added = upgrades.AddComponent(upgrade) as Upgrade;

    }
}
