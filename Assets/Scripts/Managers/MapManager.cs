/*
 * Class to manage all things related to the grid, and map generation
 * 
 * @author Richard
 * @version January 5
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //Singleton
    public static MapManager Instance;

    //Values for the grid
    enum cell {empty, floor, wall};
    enum entity {empty, player, enemy};
    cell[,] grid;
    entity[,] entityGrid;
    Vector2 spawnPos;
    int roomWidth, roomHeight;
    [Header("Grid Parameters")] 
    [SerializeField] Vector2 roomSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] float worldUnitsInOneGridCell = 1f;
    [SerializeField] GameObject wallObject, floorObject;
    [SerializeField] float minSquaredDistanceFromEnemy = 10f;

    //Walkers for algorithm
    struct walker
    {
        public Vector2 direction;
        public Vector2 position;
    }
    List<walker> walkers;
    Vector2[] cardinalDirections = {Vector2.up, Vector2.left, Vector2.down, Vector2.right};

    //Specified chances for different things to occur
    [Header("Generation Modification")]
    [SerializeField] float chanceWalkerChangeDir = 0.5f;
    [SerializeField] float chanceWalkerSpawn = 0.05f;
    [SerializeField] float chanceWalkerDestroy = 0.05f;
    [SerializeField] float chanceEnemySpawn = 0.1f;
    [SerializeField] float percentToFill = 0.2f;
    [SerializeField] int maxWalkers = 10;

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    /*
     * Destroys the map for the scene and spawns in the roomgrid from GridManager
     * 
     * @return void
     */
    public void DestroyMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        GridManager.Instance.SpawnGrid();
    }

    /*
     * Generates the map for the scene
     * 
     * @return void
     */
    public void GenerateMap()
    {
        Setup();
        CreateFloor();
        CreateWalls();
        RemoveSingleWalls();
        GenerateEnemies();
        SpawnLevel();
    }

    /*
     * Sets up and initalizes variables for the grid
     * 
     * @return void
     */
    void Setup()
    {
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);

        grid = new cell[roomWidth, roomHeight];
        entityGrid = new entity[roomWidth, roomHeight];

        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y = 0; y < roomHeight-1; y++)
            {
                grid[x, y] = cell.empty;
                entityGrid[x, y] = entity.empty;
            }
        }

        walkers = new List<walker>();

        walker newWalker = new walker();
        newWalker.direction = RandomDirection();

        spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2f), Mathf.RoundToInt(roomHeight / 2f));
        newWalker.position = spawnPos;

        entityGrid[(int) spawnPos.x, (int) spawnPos.y] = entity.player;
        walkers.Add(newWalker);
    }

    /*
     * Method that returns a random cardinal vector2 direction
     * 
     * @return Vector2 - Returns the generated vector2
     */
    Vector2 RandomDirection()
    {
        return cardinalDirections[Mathf.FloorToInt(UnityEngine.Random.Range(0f, 3.99f))];
    }

    /*
     * Method that generates the floor for the map
     * 
     * @return void
     */
    void CreateFloor()
    {
        //Failsafe
        int iterations = 0;
        do
        {
            //Make all walkers create floor
            foreach (walker myWalker in walkers)
            {
                grid[(int)myWalker.position.x, (int)myWalker.position.y] = cell.floor;
            }

            //Destroy walker?
            if (walkers.Count > 1)
            {
                int numberChecks = walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {

                    if (UnityEngine.Random.value < chanceWalkerDestroy)
                    {
                        walkers.RemoveAt(i);
                        break; //Caps destroy at one per iteration
                    }
                }
            }

            //Change direction?
            for (int i = 0; i < walkers.Count; i++)
            {
                if (UnityEngine.Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.direction = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            //Spawn new walker?
            if (walkers.Count < maxWalkers)
            {
                int numberChecks = walkers.Count;
                for (int i = 0; i < numberChecks; i++)
                {
                    if (UnityEngine.Random.value < chanceWalkerSpawn)
                    {
                        walker newWalker = new walker();
                        newWalker.direction = RandomDirection();
                        newWalker.position = walkers[i].position;
                        walkers.Add(newWalker);
                    }
                }
            }

            //Move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.position += thisWalker.direction;
                walkers[i] = thisWalker;
            }

            //Avoid grid border
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                //Clamping values to leave space for grid borders too
                thisWalker.position.x = Mathf.Clamp(thisWalker.position.x, 1, roomWidth - 2);
                thisWalker.position.y = Mathf.Clamp(thisWalker.position.y, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }

            if (NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < 100000);
    }

    /*
     * Method that counds the number of floor tiles already existing on the grid and returns it
     * 
     * @return int - Returns the number of floor tiles
     */
    int NumberOfFloors()
    {
        int count = 0;
        foreach (cell space in grid)
        {
            if (space == cell.floor) count++;
        }
        return count;
    }

    /*
     * A method that removes single walls within the map
     * 
     * @return void
     */
    void RemoveSingleWalls()
    {
        for(int x = 1; x < roomWidth - 2; x++)
        {
            for(int y = 1; y < roomHeight - 2; y++)
            {
                if (grid[x, y] == cell.wall)
                {
                    bool allFloors = (grid[x, y + 1] == cell.floor) &&
                                      (grid[x, y - 1] == cell.floor) &&
                                      (grid[x + 1, y] == cell.floor) &&
                                      (grid[x - 1, y] == cell.floor) ? true : false;

                    if (allFloors)
                    {
                        grid[x, y] = cell.floor;
                    }
                }
            }
        }
    }

    void GenerateEnemies()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == cell.floor)
                {
                    if(Vector2.SqrMagnitude(new Vector2(spawnPos.x - x, spawnPos.y - y)) > minSquaredDistanceFromEnemy)
                    {
                        if(UnityEngine.Random.value < chanceEnemySpawn) 
                        {
                            entityGrid[x, y] = entity.enemy;
                        }
                    }
                }
            }
        }
    }

    /*
     * Creates the map that was generated using gameobjects within the scene
     * 
     * @return void
     */
    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch(grid[x, y])
                {
                    case cell.empty: break;
                    case cell.floor:
                        Spawn(x, y, floorObject);
                        break;
                    case cell.wall:
                        Spawn(x, y, wallObject);
                        break;
                }
                switch(entityGrid[x, y])
                {
                    case entity.empty: break;
                    case entity.enemy:
                        //entitymanager spawn enemy
                        break;
                    case entity.player: 
                        //entitymanager spawn player
                        break;
                }
            }
        }
    }
    
    /*
     * Spawns a gameobject at a certain co-ordinate
     * 
     * @param x - X co-ordinate of the location to spawn to
     * @param y - Y co-ordinate of the location to spawn to
     * @param toSpawn - The gameobject that you want to spawn
     * @return void
     */
    void Spawn(int x, int y, GameObject toSpawn)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
        Instantiate(toSpawn, spawnPos, Quaternion.identity, transform);
    }

    /*
     * Method that generates the walls around the the generated flooring
     * 
     * @return void
     */
    void CreateWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == cell.floor)
                {
                    if (grid[x, y + 1] == cell.empty)
                    {
                        grid[x, y + 1] = cell.wall;
                    }
                    if (grid[x, y - 1] == cell.empty)
                    {
                        grid[x, y - 1] = cell.wall;
                    }
                    if (grid[x + 1, y] == cell.empty)
                    {
                        grid[x + 1, y] = cell.wall;
                    }
                    if (grid[x - 1, y] == cell.empty)
                    {
                        grid[x - 1, y] = cell.wall;
                    }
                    if (grid[x + 1, y + 1] == cell.empty)
                    {
                        grid[x + 1, y + 1] = cell.wall;
                    }
                    if (grid[x + 1, y - 1] == cell.empty)
                    {
                        grid[x + 1, y - 1] = cell.wall;
                    }
                    if (grid[x - 1, y + 1] == cell.empty)
                    {
                        grid[x - 1, y + 1] = cell.wall;
                    }
                    if (grid[x - 1, y - 1] == cell.empty)
                    {
                        grid[x - 1, y - 1] = cell.wall;
                    }
                }
            }
        }
    }
}
