using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager Instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;


    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

    }
    /**
     * Method for spawning the player, loads all data.
     * @param x X coord for spawning the player
     * @param y Y coord for spawning the player
     */
    public void SpawnPlayer(float x, float y)
    {
        Debug.Log(x + " " + y);
        player = Instantiate(playerPrefab, new Vector2(x, y), Quaternion.identity);
    }

    /**
     * Method for despawning the player, saves all data.
     */
    public void DespawnPlayer()
    {
        Destroy(player);
    }
}
