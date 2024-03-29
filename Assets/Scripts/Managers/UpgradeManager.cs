/*
 * Class for managing upgrade selection.
 * 
 * @author Richard
 * @version January 23
 */

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
    List<Upgrade> upgradeListCopy = new List<Upgrade>();
    List<Upgrade> obtainedUpgrades = new List<Upgrade>();
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

    private void Start()
    {
        for(int i = 0; i < upgradeList.Count; i++)
        {
            upgradeListCopy.Add(upgradeList[i]);
        }
    }

    /*
     * Gives a string representation of all upgrades obtained
     * 
     * return string - The representation of the obtained upgrades
     */
    public string GetObtainedUpgrades()
    {
        string output = "";
        for (int i = 0; i < obtainedUpgrades.Count; i++)
        {
            if (i == obtainedUpgrades.Count - 1)
            {
                output += obtainedUpgrades[i].upgradeName;
                break;
            }
            output += obtainedUpgrades[i].upgradeName + ", ";
        }
        return output;
    }

    /*
     * Resets the upgrade manager for use again
     */
    public void ResetUpgrades()
    {
        obtainedUpgrades.Clear();
        for (int i = 0; i < upgradeList.Count; i++)
        {
            upgradeListCopy.Add(upgradeList[i]);
        }
    }

    /**
     * Ends the level and generates upgrade cards.
     */
    public void EndLevel()
    {
        GenerateUpgradeCards(MapManager.Instance.numUpgradeRewards);
    }
    /**
     * Generates upgrade cards based on the specified number of upgrades.
     * @param numUpgrades The number of upgrade cards to generate.
     */
    public void GenerateUpgradeCards(int numUpgrades)
    {
        this.numUpgrades = numUpgrades;
        Vector3 cameraPosition = PlayerManager.Instance.player.GetComponentInChildren<Camera>().transform.position;
        for(int i = (numCards - 1) / -2; i < (numCards + 1) / 2; i++)
        {
            GameObject cardInstance = Instantiate(card, new Vector3(i * spaceBetweenCards + cameraPosition.x, cameraPosition.y, -1f), Quaternion.identity, cardHolder);
            int num = Mathf.FloorToInt(Random.Range(0, upgradeListCopy.Count - 0.01f));
            cardInstance.GetComponent<Card>().upgrade = upgradeListCopy[num];
            cardInstance.GetComponent<Card>().index = num;
        }
    }
    /**
     * Generates upgrade cards based on the previously set number of upgrades.
     */
    public void GenerateUpgradeCards()
    {
        GenerateUpgradeCards(numUpgrades);
    }
    /**
     * Handles the event of picking an upgrade card, reducing the number of remaining upgrades and destroying the cards.
     * 
     * @param removeIndex the index of the upgrade to remove from the generated list
     */
    public void PickedUpgrade(int removeIndex)
    {
        obtainedUpgrades.Add(upgradeListCopy[removeIndex]);
        upgradeListCopy.RemoveAt(removeIndex);
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
    /**
     * Destroys the upgrade cards, removing them from the card holder.
     */
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
