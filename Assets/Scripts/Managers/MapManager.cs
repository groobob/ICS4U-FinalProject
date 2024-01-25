/*
 * Class to manage all things related to the grid, and map generation
 * 
 * @author Richard
 * @version January 15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //Singleton
    public static MapManager Instance;

    //Values for the grid
    private enum cell {empty, floor, wall};
    private enum entity {empty, enemy};
    private cell[,] grid;
    private entity[,] entityGrid;
    private Vector2 spawnPos;
    private int roomWidth, roomHeight;
    public int numLevelsGenerated;
    public int numUpgradeRewards;
    public bool moneyMap = false;
    private float extraEnemySpawnRate;
    [Header("Grid Parameters")] 
    [SerializeField] private Vector2 roomSizeWorldUnits = new Vector2(30, 30);
    [SerializeField] private float worldUnitsInOneGridCell = 1f;
    [SerializeField] private GameObject wallObject, floorObject;
    [SerializeField] private float minSquaredDistanceFromEnemy = 10f;

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
    [SerializeField] private float chanceWalkerChangeDir = 0.5f;
    [SerializeField] private float chanceWalkerSpawn = 0.05f;
    [SerializeField] private float chanceWalkerDestroy = 0.05f;
    [SerializeField] private float chanceEnemySpawn = 0.1f;
    [SerializeField] private float addedChanceOnChallenge = 0.1f;
    [SerializeField] private float percentToFill = 0.2f;
    [SerializeField] private int maxWalkers = 10;

    //References
    [Header("Serialized References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private AstarPath _pathfinder;

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateMap(GridManager.room.normal);
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
        _camera.enabled = true;
        DropManager.Instance.RemoveAllDrops();
        EnemyManager.Instance.ClearAllEnemies();
        PlayerManager.Instance.DespawnPlayer();
        ProjectileManager.Instance.ClearAllProjectiles();
        GridManager.Instance.SpawnGrid();
        // enemy manager spawn normal cassh!!
    }
    /*
     * Destroys map.
     */
    public void DestroyMap(float time)
    {
        Invoke("DestroyMap", time);
    }

    /*
     * Generates the map for the scene
     * 
     * @param GridManager.room - The type of map you are generating
     * @return void
     */
    public void GenerateMap(GridManager.room type)
    {
        numUpgradeRewards = 1;
        moneyMap = false;
        switch(type)
        {
            default:
                break;
            case GridManager.room.money:
                moneyMap = true;
                break;
            case GridManager.room.challenge:
                extraEnemySpawnRate = addedChanceOnChallenge;
                break;
            case GridManager.room.item:
                numUpgradeRewards = 2;
                break;
        }

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
    private void Setup()
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

        walkers.Add(newWalker);
    }

    /*
     * Method that returns a random cardinal vector2 direction
     * 
     * @return Vector2 - Returns the generated vector2
     */
    private Vector2 RandomDirection()
    {
        return cardinalDirections[Mathf.FloorToInt(Random.Range(0f, 3.99f))];
    }

    /*
     * Method that generates the floor for the map
     * 
     * @return void
     */
    private void CreateFloor()
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

                    if (Random.value < chanceWalkerDestroy)
                    {
                        walkers.RemoveAt(i);
                        break; //Caps destroy at one per iteration
                    }
                }
            }

            //Change direction?
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
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
                    if (Random.value < chanceWalkerSpawn)
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
    private int NumberOfFloors()
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

    private void GenerateEnemies()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == cell.floor)
                {
                    if(Vector2.SqrMagnitude(new Vector2(spawnPos.x - x, spawnPos.y - y)) > minSquaredDistanceFromEnemy)
                    {
                        if(Random.value < chanceEnemySpawn + extraEnemySpawnRate) 
                        {
                            entityGrid[x, y] = entity.enemy;
                        }
                    }
                }
            }
        }
        extraEnemySpawnRate = 0;
    }

    /*
     * Creates the map that was generated using gameobjects within the scene
     * 
     * @return void
     */
    private void SpawnLevel()
    {
        // Spawn player first so that enemys can reference it
        _camera.enabled = false;
        GameObject player = Spawn((int)spawnPos.x, (int) spawnPos.y, 0);
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
                        GameObject spawnedEnemy = Spawn(x, y, 1);
                        spawnedEnemy.GetComponent<Enemy>().target = player.transform; 
                        break;
                }
            }
        }

        // Refresh the pathfinder as there is a new map
        _pathfinder.Scan();
        DataManager.Instance.StartTimer();
        numLevelsGenerated++;
    }

    /*
     * Spawns a gameobject at a certain co-ordinate purely for environments
     * 
     * @param x - X co-ordinate of the location to spawn to
     * @param y - Y co-ordinate of the location to spawn to
     * @param toSpawn - The gameobject that you want to spawn
     * @param type - An integer for the type to spawn, 0 for environment, 1 for player, 2 for enemies
     * @return void
     */
    private void Spawn(int x, int y, GameObject toSpawn)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
        Instantiate(toSpawn, spawnPos, Quaternion.identity, transform);
    }

    /*
     * Spawns a gameobject at a certain co-ordinate purely for entities
     * 
     * @param x - X co-ordinate of the location to spawn to
     * @param y - Y co-ordinate of the location to spawn to
     * @param type - An integer for the type to spawn, 0 for player, 1 for enemies
     * @return GameObject - Returns a reference to the spawned gameobject
     */
    private GameObject Spawn(int x, int y, int type)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
        if (type == 0) return PlayerManager.Instance.SpawnPlayer(spawnPos.x, spawnPos.y);
        else return EnemyManager.Instance.SpawnEnemy(spawnPos.x, spawnPos.y, numLevelsGenerated);
    }

    /*
     * Method that generates the walls around the the generated flooring
     * 
     * @return void
     */
    private void CreateWalls()
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
