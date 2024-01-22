using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSoul : Upgrade
{

    private bool Used;
    private float iframeDuration = 4f;

    private float movementBuff = 1.5f;
    protected PlayerStats _playerStats;
    protected PlayerController _playerController;

    public void Start()
    {
        _playerStats = transform.parent.gameObject.GetComponent<PlayerStats>();
        _playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        Used = false;
        healthBoost = 1;
        classification = "Unique";
        upgradeName = "Second Soul";
        description = "Upon taking lethal damage, enter a spectral form where you cannot be damaged and gain a speed boost. Additionally gain health. Works once per level.";
    }

    public void UpgradeProcc()
    {
        if (!Used)
        {
            Used = true;
            _playerStats.GiveIFrames(iframeDuration);
            _playerStats.SpeedBoost(movementBuff, iframeDuration * 1.5f);
            _playerStats.EndlagEntity(iframeDuration / 8);
        }
    }
}
