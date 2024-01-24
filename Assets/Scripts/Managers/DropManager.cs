/*
 * A manager resposible for everything that an enemy can drop on kill
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance;

    // References
    [Header("References")]
    [SerializeField] private GameObject money;
    [SerializeField] private GameObject healthPack;

    // Values
    [Header("Values")]
    [SerializeField] private int moneyMin;
    [SerializeField] private int moneyMax;
    [SerializeField] private float healthPackDropChance;
    [SerializeField] private float launchForce;

    private void Awake()
    {
        Instance = this;
    }

    /*
     * Spawns a number of drops at a given transform
     * 
     * @param Transform - The location to spawn the object at
     */
    public void SpawnDrop(Transform pos)
    {
        Vector2 direction = new Vector2(Random.value, Random.value);

        GameObject spawnedDrop;
        if (Random.value < healthPackDropChance)
        {
            spawnedDrop = Instantiate(healthPack, pos.position, Quaternion.identity, transform);
        }
        else
        {
            spawnedDrop = Instantiate(money, pos.position, Quaternion.identity, transform);
            spawnedDrop.GetComponent<MoneyDrop>().value = Mathf.RoundToInt(Random.Range(moneyMin, moneyMax));
        }

        spawnedDrop.GetComponent<Rigidbody2D>().AddForce(direction * launchForce, ForceMode2D.Impulse);
    }

    /*
     * Clears out all the drops on the map
     */
    public void RemoveAllDrops()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
