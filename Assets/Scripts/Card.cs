using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Upgrade upgrade;
    public bool picked = false;

    private void Start()
    {
        
    }

    private void OnMouseDown()
    {
        picked = true;
        PlayerManager.Instance._playerStats.AddUpgrades(upgrade.GetComponent<Upgrade>().GetType());
        UpgradeManager.Instance.PickedUpgrade();
    }
}
