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
    [SerializeField] private List<GameObject> projectiles;

    private void Awake()
    {
        Instance = this;
    }
    /**
     * Retrieves the list of projectiles.
     * @return List<GameObject>
     */
    public List<GameObject> GetProjectileList()
    {
        return projectiles;
    }
    /**
     * Spawns a projectile at the specified position with the given force and type.
     * @param position The position to spawn the projectile.
     * @param force The force to apply to the projectile.
     * @param projectileType The type of projectile to spawn.
     */
    public void SpawnProjectile(Vector2 position, Vector2 force, int projectileType)
    {
        GameObject projectile = Instantiate(projectiles[projectileType], position, Quaternion.identity, transform);
        projectile.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
    /**
     * Spawns a projectile at the specified position with the given force, type, and rotation.
     * @param position The position to spawn the projectile.
     * @param force The force to apply to the projectile.
     * @param projectileType The type of projectile to spawn.
     * @param rotation The rotation of the projectile.
     */
    public void SpawnProjectile(Vector2 position, Vector2 force, int projectileType, Quaternion rotation)
    {
        GameObject projectile = Instantiate(projectiles[projectileType], position, rotation, transform);
        projectile.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
    /**
     * Spawns multiple projectiles in a spread pattern at the specified position with the given force, type, and spread.
     * @param position The position to spawn the projectiles.
     * @param force The force to apply to the projectiles.
     * @param projectileType The type of projectile to spawn.
     * @param projectileNum The number of projectiles to spawn.
     * @param spreadInRadians The spread angle in radians.
     */
    public void SpawnProjectileSpread(Vector2 position, Vector2 force, int projectileType, int projectileNum, float spreadInRadians)
    {
        float offset = projectileNum % 2 == 1 ? projectileNum / 2 * spreadInRadians * -1 : projectileNum / 2 * spreadInRadians * -1 + spreadInRadians / 2;
        for (int i = 0; i < projectileNum; i++)
        {
            GameObject projectile = Instantiate(projectiles[projectileType], position, Quaternion.identity, transform);
            projectile.GetComponent<Rigidbody2D>().AddForce(rotateVector2(force, spreadInRadians * i + offset) * 45);
        }
    }

    /**
     * Rotates a 2D vector by the specified angle in radians.
     * @param v The 2D vector to rotate.
     * @param radians The angle in radians to rotate the vector.
     * @return Vector2
     */
    public static Vector2 rotateVector2(Vector2 v, float radians)
    {
        return new Vector2(
            v.x * Mathf.Cos(radians) - v.y * Mathf.Sin(radians),
            v.x * Mathf.Sin(radians) + v.y * Mathf.Cos(radians)
        );
    }
    /**
     * Clears all projectiles by destroying them.
     */
    public void ClearAllProjectiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
