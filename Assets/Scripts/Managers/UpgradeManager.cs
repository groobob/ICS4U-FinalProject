using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Singleton
    public static UpgradeManager Instance;
    // References
    [Header("References")]
    [SerializeField] List<Upgrade> upgradeList;
    [SerializeField] GameObject card;
    [SerializeField] Transform cardHolder;

    [Header("Values")]
    [SerializeField] int numCards;
    [SerializeField] float spaceBetweenCards;
    [SerializeField] float timeAfterCardSelection;
    int numUpgrades;

    private void Awake()
    {
        Instance = this;
    }

    public void EndLevel()
    {
        GenerateUpgradeCards(MapManager.Instance.numUpgradeRewards);
    }

    public void GenerateUpgradeCards(int numUpgrades)
    {
        this.numUpgrades = numUpgrades;
        Vector3 cameraPosition = PlayerManager.Instance.player.GetComponentInChildren<Camera>().transform.position;
        for(int i = (numCards - 1) / -2; i < (numCards + 1) / 2; i++)
        {
            GameObject cardInstance = Instantiate(card, new Vector3(i * spaceBetweenCards + cameraPosition.x, cameraPosition.y, 0), Quaternion.identity, cardHolder);
            cardInstance.GetComponent<Card>().upgrade = upgradeList[Mathf.FloorToInt(Random.Range(0, upgradeList.Count - 0.01f))];
        }
    }

    public void GenerateUpgradeCards()
    {
        GenerateUpgradeCards(numUpgrades);
    }

    public void PickedUpgrade()
    {
        numUpgrades--;
        DestroyCards();
        if (numUpgrades <= 0)
        {
            MapManager.Instance.DestroyMap(timeAfterCardSelection);
        }
        else
        {
            Invoke("GenerateUpgradeCards", timeAfterCardSelection);
        }
    }

    public void DestroyCards()
    {
        for (int i = cardHolder.childCount - 1; i >= 0; i--)
        {
            var child = cardHolder.GetChild(i).gameObject;
            if (!child.GetComponent<Card>().picked) Destroy(child);
            else Destroy(child, timeAfterCardSelection);
        }
    }
}
