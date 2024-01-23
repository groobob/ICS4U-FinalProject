using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] GameObject nameText;
    [SerializeField] GameObject descriptionText;

    // Values
    public Upgrade upgrade;
    public bool picked;

    private void Start()
    {
        nameText.GetComponent<TextMeshPro>().text = upgrade.upgradeName;
        descriptionText.GetComponent<TextMeshPro>().text = upgrade.description;
        picked = false;
    }

    private void OnMouseDown()
    {
        picked = true;
        PlayerManager.Instance._playerStats.AddUpgrades(upgrade.GetComponent<Upgrade>().GetType());
        UpgradeManager.Instance.PickedUpgrade();
    }
}
