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
    int numEnemies = 0;
    void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnEnemy(float x, float y)
    {
        // placeholder math later
        numEnemies++;
        return Instantiate(tierOneEnemies[0], new Vector2(x, y), Quaternion.identity);

    }
}
