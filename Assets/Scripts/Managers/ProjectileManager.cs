/*
 * A class for managing projectile spawning
 * 
 * @author Richard
 * @version January 18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [Header("References")]
    [SerializeField] List<GameObject> projectiles;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnProjectile(Vector2 position, Vector2 force, int projectileType)
    {
        GameObject projectile = Instantiate(projectiles[projectileType], position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
