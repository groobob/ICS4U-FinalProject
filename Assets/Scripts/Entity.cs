/*
 * Superclass to all Entities. Handles base stats for all entities.
 * 
 * @author Evan
 * @version January 09
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Values")]
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;
    [SerializeField] public float baseMoveSpeed;
    [SerializeField] protected Rigidbody2D _rb;

    public void setBaseStats(int HP, float speed)
    {
        maxHealth = HP;
        health = maxHealth;
        this.baseMoveSpeed = speed;
    }

    // >>>>>>>>> INSTANCE METHODS <<<<<<<<<
    /**
     * Method representing what occurs when the entity dies.
     */
    public abstract void DeathEvent();
    /**
     * Method that adds an entity to the world
     */
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathEvent();
        }
    }
    /**
     * Method for making an entity heal damage.
     * @param damage Damage a Unit should heal.
     */
    public void HealDamage(int damage)
    {
        health += damage;
        if (health > maxHealth)
        {
            maxHealth = health;
        }
    }
    /**
     * Method for setting entity speed.
     * @param spd Speed to set
     */
    public void SetBaseSpeed(float spd)
    { this.baseMoveSpeed = spd; }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMoveSpeed()
    {
        return baseMoveSpeed;
    }
    
}
