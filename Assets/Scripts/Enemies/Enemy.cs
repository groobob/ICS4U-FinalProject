/*
 * Superclass to all Enemies. Inherets from Entity.
 * 
 * @author Evan, Richard
 * @version January 23
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : Entity
{
    public Transform target;
    public Transform sprite;
    [Header("Enemy Values")]
    [SerializeField] protected float detectionRadiusSquared;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float meleeRange;
    [SerializeField] protected GameObject enemyTargetIndicator;
    [SerializeField] protected float enemyTargetSpaceBetweenPlayer;
    [SerializeField] private float flashSpeed;

    float flashingTime;
    bool transparent = false;
    protected bool dead = false; 
    private float nextAttackTime;

    private void Awake()
    {
        flashingTime = Random.value * flashSpeed;
    }
    private void Update()
    {
        // Position calculation for player
        if (enemyTargetIndicator == null) return;
        enemyTargetIndicator.transform.position = target.position - (target.position - transform.position).normalized * enemyTargetSpaceBetweenPlayer;

        // Flashing target indicators
        flashingTime += Time.deltaTime;
        if(!transparent)
        {
            if(flashingTime > flashSpeed)
            {
                transparent = true;
                flashingTime = 0;
                return;
            }
            enemyTargetIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, flashingTime / flashSpeed);
        }
        else
        {
            if (flashingTime > flashSpeed)
            {
                transparent = false;
                flashingTime = 0;
                return;
            }
            enemyTargetIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (flashSpeed - flashingTime) / flashSpeed);
        }

    }

    /**
     * Runs whenever the enemy dies
     */
    public override void DeathEvent()
    {
        DataManager.Instance.IncrementData(DataManager.stats.kills);

        // Change later
        Death();
        EnemyManager.Instance.DecreaseEnemyNumber();
        Destroy(gameObject, 1f);
    }
    /**
     * Increases HP for every room cleared.
     * @param n An integer representing the number of rooms done
     */
    public void BoostHealthPerLevel(int n)
    {
        health += 2 * n;
        maxHealth += 2 * n;
    }

    protected abstract void Attack();

    protected abstract void Death();

    protected void AttackCheck()
    {
        if (nextAttackTime < Time.time)
        {
            Attack();
            nextAttackTime = Time.time + attackSpeed;
        }
    }
}
