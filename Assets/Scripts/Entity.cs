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
    [SerializeField] public float knockbackResistance = 1f;
    [SerializeField] protected Rigidbody2D _rb;

    [SerializeField] public float speedFactor = 1f;

    // Stun variables

    private float rootReleaseTime;
    private float stunReleaseTime;
    private float endlagReleaseTime;

    public void Start()
    {
        
    }

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

    public void StunEntity(float duration)
    {
        stunReleaseTime = Time.time + duration;
    }

    public float GetStunReleaseTime()
    {
        return stunReleaseTime;
    }

    public void EndlagEntity(float duration)
    {
        endlagReleaseTime = Time.time + duration;
    }

    public float GetEndlagReleaseTime()
    {
        return endlagReleaseTime;
    }

    public void SpeedBoost(float factor, float duration) // ALSO WORKS FOR SLOWS
    {
        StartCoroutine(speedChange(factor, duration));
    }

    private IEnumerator speedChange(float factor, float duration)
    {
        speedFactor *= factor;
        yield return new WaitForSeconds(duration);
        speedFactor /= factor;
    }

    public float ApplySpeedMods()
    {
        float speedMultiplier = 1;
        speedMultiplier *= (speedFactor);
        return speedMultiplier;
    }

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
            maxHealth = health;
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

    public float GetRootReleaseTime()
    {
        return rootReleaseTime;
    }

    public void RootEntity(float duration)
    {
        rootReleaseTime = Time.time + duration;
    }

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

    public void CreateDamageNumber(int amount, bool type)
    {
        GameObject dmgNum = Instantiate(PlayerManager.Instance.damageNumbers, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        dmgNum.GetComponent<DamageNumScript>().SetNumber(amount, type);
    }
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
