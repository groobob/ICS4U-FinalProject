/*
 * Class to manage all things related to the grid of rooms between maps
 * 
 * @author Richard
 * @version January 5
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Singleton
    public static GridManager Instance;

    //Values for the grid
    public enum room {normal, item, money, challenge, explored, current, end, empty};
    private room[,] roomGrid;
    private Vector2Int playerPosition;
    [Header("Grid Parameters")]
    [SerializeField] private Vector2 gridSize = new Vector2(10, 10);
    [SerializeField] private int gridSpacing = 10;
    [SerializeField] private GameObject roomObject;

    //Specified chances for different things to occur
    [Header("Generation Modification")]
    [SerializeField] private float chanceItem = 0.1f;
    [SerializeField] private float chanceShop = 0.1f;
    [SerializeField] private float chanceChallenge = 0.1f;
    
    /*
     * Method is called right when script is loaded
     * 
     * @return void
     */
    private void Awake()
    {
        Instance = this;
    }

    /*
     * Method called before the first updating frame
     * 
     * @return void
     */
    private void Start()
    {
        SetupGrid();
    }

    /*
     * Generates the grid for the rooms
     * 
     * @return void
     */
    void SetupGrid()
    {
        roomGrid = new room[(int) gridSize.x, (int) gridSize.y];
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                // Left side and right side is pregenerated
                // Left side
                if (x == 0 && y == (gridSize.y - 1) / 2)
                {
                    roomGrid[x, y] = room.current;
                    playerPosition = new Vector2Int(0, y);
                    continue;
                }
                else if (x == 0)
                {
                    roomGrid[x, y] = room.empty;
                    continue;
                }

                // Right side
                if (x == gridSize.x - 1 && y == (gridSize.y - 1) / 2)
                {
                    roomGrid[x, y] = room.end;
                    playerPosition = new Vector2Int(0, y);
                    continue;
                }
                else if (x == gridSize.x - 1 && !(y == (gridSize.y - 1) / 2))
                {
                    roomGrid[x, y] = room.empty;
                    continue;
                }

                float randomNum = Random.value;
                if (randomNum < chanceItem) roomGrid[x, y] = room.item;
                else if (randomNum < chanceItem + chanceShop) roomGrid[x, y] = room.money;
                else if (randomNum < chanceItem + chanceShop + chanceChallenge) roomGrid[x, y] = room.challenge;
                else roomGrid[x, y] = room.normal;
            }
        }
    }

    /*
     * Destroys the spawned grid and generates the next level/map for the player to play on
     * 
     * @return void
     */
    public void DestroyGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    /*
     * Spawns the grid onto the scene with objects
     * 
     * @return void
     */
    public void SpawnGrid()
    {
        Vector2 offset = new Vector2((gridSize.x - 1) / 2, (gridSize.y - 1) / 2);
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 spawnPos = (new Vector2(x, y) - offset) * gridSpacing;
                GameObject instance = Instantiate(roomObject, spawnPos, Quaternion.identity, transform);
                Room _room = instance.GetComponent<Room>();
                bool isNeighbour = (x == playerPosition.x + 1) && (y == playerPosition.y + 2) ||
                                    (x == playerPosition.x + 1) && (y == playerPosition.y + 1) ||
                                    (x == playerPosition.x + 1) && (y == playerPosition.y) ||
                                    (x == playerPosition.x + 1) && (y == playerPosition.y - 1) ||
                                    (x == playerPosition.x + 1) && (y == playerPosition.y - 2) ? true : false;

                switch (roomGrid[x, y])
                {
                    case room.normal:
                        _room.Setup(room.normal, isNeighbour, x, y);
                        break;
                    case room.item:
                        _room.Setup(room.item, isNeighbour, x, y);
                        break;
                    case room.money:
                        _room.Setup(room.money, isNeighbour, x, y);
                        break;
                    case room.challenge:
                        _room.Setup(room.challenge, isNeighbour, x, y);
                        break;
                    case room.explored:
                        _room.Setup(room.explored, false, x, y);
                        break;
                    case room.current:
                        _room.Setup(room.current, false, x, y);
                        break;
                    case room.end:
                        _room.Setup(room.end, isNeighbour, x, y);
                        break;
                    case room.empty:
                        _room.Setup(room.empty, false, x, y);
                        break;
                }
            }
        }
    }

    /*
     * Call this function to move the player on the room grid to a specified x and y coordinate
     * 
     * @param x The x coordinate to move to
     * @param y The y coordinate to move to
     * @return void
     */
    public void Move(int x, int y)
    {
        roomGrid[playerPosition.x, playerPosition.y] = room.explored;
        roomGrid[x, y] = room.current;
        playerPosition.x = x;
        playerPosition.y = y;
    }
}
