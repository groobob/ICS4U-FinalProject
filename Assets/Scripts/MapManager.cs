using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //The grid
    enum cell {empty, floor, wall};
    cell[,] grid;
    int roomWidth, roomHeight;
    [Header("Grid Parameters")] 
    [SerializeField] Vector2 roomSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] float worldUnitsInOneGridCell = 1;
    [SerializeField] GameObject wallObject, floorObject;

    //Walkers
    struct walker
    {
        public Vector2 direction;
        public Vector2 position;
    }
    List<walker> walkers;
    Vector2[] cardinalDirections = {Vector2.up, Vector2.left, Vector2.down, Vector2.right};

    //Chances
    [Header("Generation Modification")]
    [SerializeField] float chanceWalkerChangeDir = 0.5f;
    [SerializeField] float chanceWalkerSpawn = 0.05f;
    [SerializeField] float chanceWalkerDestroy = 0.05f;
    [SerializeField] float percentToFill = 0.2f;
    [SerializeField] int maxWalkers = 10;

    void Start()
    {
        Setup();
        CreateFloor();
        CreateWalls();
        SpawnLevel();
    }

    void Setup()
    {
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
        Debug.Log(roomHeight - 2);
        Debug.Log(roomWidth - 2);

        grid = new cell[roomWidth, roomHeight];

        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y = 0; y < roomHeight-1; y++)
            {
                grid[x, y] = cell.empty;
            }
        }

        walkers = new List<walker>();

        walker newWalker = new walker();
        newWalker.direction = RandomDirection();

        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2f), Mathf.RoundToInt(roomHeight / 2f));
        newWalker.position = spawnPos;

        walkers.Add(newWalker);
    }

    Vector2 RandomDirection()
    {
        return cardinalDirections[Mathf.FloorToInt(UnityEngine.Random.Range(0f, 3.99f))];
    }

    void CreateFloor()
    {
        //Failsafe
        int iterations = 0;
        do
        {
            //Make all walkers create floor
            foreach (walker myWalker in walkers)
            {
                Debug.Log((int)myWalker.position.x + ", " + (int)myWalker.position.y);
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

    int NumberOfFloors()
    {
        int count = 0;
        foreach (cell space in grid)
        {
            if (space == cell.floor) count++;
        }
        return count;
    }

    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch(grid[x,y])
                {
                    case cell.empty: break;
                    case cell.floor:
                        Spawn(x, y, floorObject);
                        break;
                    case cell.wall:
                        Spawn(x, y, wallObject);
                        break;
                }
            }
        }
    }
    
    void Spawn(int x, int y, GameObject toSpawn)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

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
                }
            }
        }
    }
}
