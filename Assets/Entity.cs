using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int health;
    protected int maxHealth;

    /**
     * Constructor for Entity.
     * @param HP Max Health for entity.
     */
    public Entity(int HP)
    {
        maxHealth = HP;
        health = maxHealth;
    }

    // >>>>>>>>> INSTANCE METHODS <<<<<<<<<
    /**
     * Method representing what occurs when the entity dies.
     */
    public abstract void DeathEvent();
    /**
     * Method that adds an entity to the world
     */
    public abstract void AddEntity();
    /**
     * Method that removes an entity from the world.
     */
    public abstract void RemoveEntity();
    /**
     * Method for making an entity take damage.
     * @param damage Damage a Unit should take.
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
}
