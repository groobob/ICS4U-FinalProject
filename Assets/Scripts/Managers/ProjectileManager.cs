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
        GameObject projectile = Instantiate(projectiles[projectileType], position, Quaternion.identity, transform);
        projectile.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    public void SpawnProjectileSpread(Vector2 position, Vector2 force, int projectileType, int projectileNum, float spreadInRadians)
    {
        float offset = projectileNum % 2 == 1 ? projectileNum / 2 * spreadInRadians * -1 : projectileNum / 2 * spreadInRadians * -1 + spreadInRadians / 2;
        for (int i = 0; i < projectileNum; i++)
        {
            GameObject projectile = Instantiate(projectiles[projectileType], position, Quaternion.identity, transform);
            projectile.GetComponent<Rigidbody2D>().AddForce(rotateVector2(force, spreadInRadians * i + offset) * 45);
        }
    }

    // rotates ccw
    public static Vector2 rotateVector2(Vector2 v, float radians)
    {
        return new Vector2(
            v.x * Mathf.Cos(radians) - v.y * Mathf.Sin(radians),
            v.x * Mathf.Sin(radians) + v.y * Mathf.Cos(radians)
        );
    }

    public void ClearAllProjectiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
