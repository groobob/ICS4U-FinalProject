using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected bool breakOnWalls;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (breakOnWalls)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        Destroy(gameObject, 4f);
    }
}
