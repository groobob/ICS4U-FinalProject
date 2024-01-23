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
    [SerializeField] List<GameObject> tierOneEnemies;
    [SerializeField] List<GameObject> tierTwoEnemies;
    [SerializeField] List<GameObject> tierThreeEnemies;

    // Values
    float tierOneMaxSpawnRate = -4 * 10 + 60;
    float tierTwoMaxSpawnRate = -10 + 40;
    //float tierThreeMaxSpawnRate = 5 * 10;
    int numEnemies = 0;
    //List<Enemy> enemies = new List<Enemy>();
    void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnEnemy(float x, float y, int numLevelsGenerated)
    {
        numEnemies++;

        float chanceTier1 = numLevelsGenerated >= 10 ? tierOneMaxSpawnRate : -4 * numLevelsGenerated + 60;
        float chanceTier2 = numLevelsGenerated >= 10 ? tierTwoMaxSpawnRate : -numLevelsGenerated + 40;
        int random = Mathf.FloorToInt(Random.Range(0, 100.999f));

        // tier 1, 2, then 3
        if (random < chanceTier1) return Instantiate(tierOneEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
        else if(random < chanceTier1 + chanceTier2) return Instantiate(tierTwoEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
        else return Instantiate(tierThreeEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);       
    }

    //Used as a placeholder for later
    public void ClearAllEnemies()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        numEnemies = 0;
    }

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
