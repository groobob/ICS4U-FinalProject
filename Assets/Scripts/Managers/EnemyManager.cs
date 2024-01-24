/*
 * Manager class to manage all the enemies with spawning and destroying
 * 
 * @author Richard
 * @version January 19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Singleton
    public static EnemyManager Instance;

    // Serialized References
    [Header("Enemy Game Objects")]
    [SerializeField] private List<GameObject> tierOneEnemies;
    [SerializeField] private List<GameObject> tierTwoEnemies;
    [SerializeField] private List<GameObject> tierThreeEnemies;

    // Values
    private float tierOneMaxSpawnRate = -4 * 10 + 60;
    private float tierTwoMaxSpawnRate = -10 + 40;
    //float tierThreeMaxSpawnRate = 5 * 10;
    private int numEnemies = 0;
    //List<Enemy> enemies = new List<Enemy>();
    private void Awake()
    {
        Instance = this;
    }
    /**
     * Spawns an enemy using specific parameters. Returns spawned enemy.
     * @param x X coord of enemy
     * @param y Y coord of enemy
     * @param numLevelsGenerated The amount of levels currently generated
     * @return GameObject
     */
    public GameObject SpawnEnemy(float x, float y, int numLevelsGenerated)
    {
        numEnemies++;

        float chanceTier1 = numLevelsGenerated >= 10 ? tierOneMaxSpawnRate : -4 * numLevelsGenerated + 60;
        float chanceTier2 = numLevelsGenerated >= 10 ? tierTwoMaxSpawnRate : -numLevelsGenerated + 40;
        int random = Mathf.FloorToInt(Random.Range(0, 100.999f));

        // tier 1, 2, then 3
        if (random < chanceTier1)
        {
            GameObject obj = Instantiate(tierOneEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
            obj.GetComponent<Enemy>().BoostHealthPerLevel(MapManager.Instance.numLevelsGenerated);
            return obj;
        }
        else if (random < chanceTier1 + chanceTier2)
        {
            GameObject obj = Instantiate(tierTwoEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
            obj.GetComponent<Enemy>().BoostHealthPerLevel(MapManager.Instance.numLevelsGenerated);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(tierThreeEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
            obj.GetComponent<Enemy>().BoostHealthPerLevel(MapManager.Instance.numLevelsGenerated);
            return obj;
        }
    }

    /**
     * Clears all enemies from the game.
     */
    public void ClearAllEnemies()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        numEnemies = 0;
    }
    /**
     * Decreases the number of enemies.
     */
    public void DecreaseEnemyNumber()
    {
        numEnemies--;
        if (numEnemies <= 0)
        {
            PlayerManager.Instance.DisablePlayerControls();
            UpgradeManager.Instance.GenerateUpgradeCards(MapManager.Instance.numUpgradeRewards);
        }
    }
}
