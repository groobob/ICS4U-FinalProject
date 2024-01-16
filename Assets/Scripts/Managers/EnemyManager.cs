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
    List<Enemy> enemies = new List<Enemy>();
    void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnEnemy(float x, float y)
    {
        // placeholder math later
        numEnemies++;
        return Instantiate(tierOneEnemies[Mathf.FloorToInt(Random.Range(0f, 1.999f))], new Vector2(x, y), Quaternion.identity, transform);
    }

    //Used as a placeholder for later
    public void ClearAllEnemies()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
