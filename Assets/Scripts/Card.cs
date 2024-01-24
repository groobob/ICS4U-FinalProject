/*
 * Class for picking upgrade cards.
 * 
 * @author Richard
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private GameObject nameText;
    [SerializeField] private GameObject descriptionText;

    // Values
    public Upgrade upgrade;
    public bool picked;
    public int index;

    private void Start()
    {
        nameText.GetComponent<TextMeshPro>().text = upgrade.upgradeName;
        descriptionText.GetComponent<TextMeshPro>().text = upgrade.description;
        picked = false;
    }

    private void OnMouseDown()
    {
        picked = true;
        SoundManager.Instance.PlayAudio(16);
        PlayerManager.Instance._playerStats.AddUpgrades(upgrade.GetComponent<Upgrade>().GetType());
        UpgradeManager.Instance.PickedUpgrade(index);
        DataManager.Instance.UpdateUpgradesObtainedList();
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
