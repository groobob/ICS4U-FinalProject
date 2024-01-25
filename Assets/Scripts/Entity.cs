/*
 * Superclass to all Entities. Handles base stats for all entities.
 * 
 * @author Evan
 * @version January 23
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
    [SerializeField] public float knockbackResistance = 1f;
    [SerializeField] protected Rigidbody2D _rb;

    [SerializeField] public float speedFactor = 1f;

    // Stun variables

    private float rootReleaseTime;
    private float stunReleaseTime;
    private float endlagReleaseTime;


    // >>>>>>>>> INSTANCE METHODS <<<<<<<<<
    /**
     * Method representing what occurs when the entity dies.
     */
    public abstract void DeathEvent();
    /**
     * Method that adds an entity to the world
     */
    public bool TakeDamage(int damage)
    {
        if (health <= 0) { return false; } // if hitting dead
        health -= damage;
        CreateDamageNumber(damage, true);
        if (health <= 0)
        {
            DeathEvent();
        }
        return true;
    }
    /**
     * Stuns the entity for a specified duration.
     * @param duration The duration of the stun effect.
     */

    public void StunEntity(float duration)
    {
        stunReleaseTime = Time.time + duration;
    }
    /**
     * Retrieves the time at which the stun effect is released.
     * @return float
     */
    public float GetStunReleaseTime()
    {
        return stunReleaseTime;
    }
    /**
     * Initiates an endlag for the entity for a specified duration.
     * @param duration The duration of the endlag.
     */
    public void EndlagEntity(float duration)
    {
        endlagReleaseTime = Time.time + duration;
    }

    /**
     * Retrieves the time at which the endlag effect is released.
     * @return The time at which the endlag effect is released.
     */
    public float GetEndlagReleaseTime()
    {
        return endlagReleaseTime;
    }
    /**
     * Provides a speed boost or slow effect to the entity for a specified duration.
     * @param factor The factor by which the speed is modified.
     * @param duration The duration of the speed modification effect.
     */
    public void SpeedBoost(float factor, float duration) // ALSO WORKS FOR SLOWS
    {
        StartCoroutine(speedChange(factor, duration));
    }
    private IEnumerator speedChange(float factor, float duration)
    {
        speedFactor *= factor;
        yield return new WaitForSeconds(duration); // waits time before setting speed back
        speedFactor /= factor;
    }
    /**
     * Retrieves the speed multiplier applied to the entity.
     * @return float
     */
    public float ApplySpeedMods()
    {
        float speedMultiplier = 1;
        speedMultiplier *= (speedFactor);
        return speedMultiplier;
    }
    /**
     * Checks if the entity is disabled due to stun or endlag effects.
     * @return bool
     */
    public bool checkDisabled() // returns true if valid
    {
        if (stunReleaseTime > Time.time || endlagReleaseTime > Time.time)
        {
            return false;
        }
        return true;
    }

    /**
     * Method for making an entity heal damage.
     * @param damage Damage a Unit should heal.
     */
    public void HealDamage(int damage)
    {
        if (health + damage > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += damage;
        }
    }
    /**
     * Method for setting entity speed.
     * @param spd Speed to set
     */
    public void SetBaseSpeed(float spd)
    { this.baseMoveSpeed = spd; }
    /**
     * Retrieves the time at which the root effect is released.
     * @return The time at which the root effect is released.
     */
    public float GetRootReleaseTime()
    {
        return rootReleaseTime;
    }
    /**
     * Initiates a root effect on the entity for a specified duration.
     * @param duration The duration of the root effect.
     */
    public void RootEntity(float duration)
    {
        rootReleaseTime = Time.time + duration;
    }
    /**
     * Applies a knockback effect to the entity from a specified sender.
     * @param sender The GameObject initiating the knockback.
     * @param strength The strength of the knockback effect.
     * @param duration The duration of the knockback effect.
     */
    public void GiveKnockBack(GameObject sender, float strength, float duration)
    {
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        _rb.AddForce(strength * direction * knockbackResistance, ForceMode2D.Impulse);
        ResetKnockback(duration);
        //Debug.Log("Knockback");
    }

    private IEnumerator ResetKnockback(float duration)
    {
        yield return new WaitForSeconds(duration);
        _rb.velocity = Vector3.zero;
    }
    /**
     * Creates a damage number visual representation for the entity.
     * @param amount The amount of damage to display.
     * @param type The type of damage (e.g., true for critical damage).
     */
    public void CreateDamageNumber(int amount, bool type)
    {

        float xPos = UnityEngine.Random.Range(-70, 71) / 100f;;
        GameObject dmgNum = Instantiate(PlayerManager.Instance.damageNumbers, transform.position + new Vector3(xPos, 1, 0), Quaternion.identity);
        dmgNum.GetComponent<DamageNumScript>().SetNumber(amount, type);
    }
    /**
     * Retrieves the current health of the entity.
     * @return int
     */
    public int GetHealth()
    {
        return health;
    }
    /**
     * Retrieves the maximum health of the entity.
     * @return int
     */
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /**
     * Retrieves the base movement speed of the entity.
     * @return float
     */
    public float GetMoveSpeed()
    {
        return baseMoveSpeed;
    }
    
}
