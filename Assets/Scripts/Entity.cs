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
    protected int health;
    protected int maxHealth;
    protected float speed;

    /**
     * Constructor for Entity.
     * @param HP Max Health for entity.
     */
    public Entity(int HP, float speed)
    {
        maxHealth = HP;
        health = maxHealth;
        this.speed = speed;
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
    public void SetSpeed(float spd)
    { this.speed = spd; }

    /**
     * Method for giving entity a timed speed boost
     * @param spd Speed to set
     * @param delay time before speed returns to before.
     * 
     * DO NOT USE THIS, TELL ME THAT U CAN STACK SPEED BOOSTS. IFYOU CALL THEM CONSECUATIVELY THEY BUG OUT
     */
    public void SpeedBoost(float spd, float delay)
    {
        float tempSpeed;
        tempSpeed = speed;
        SetSpeed(spd);
        Invoke("SetSpeed(tempSpeed)", delay);

    }
}
