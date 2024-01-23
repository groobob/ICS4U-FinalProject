/*
 * Superclass to all Enemies. Inherets from Entity.
 * 
 * @author Evan
 * @version January 09
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
    private float nextAttackTime;

    private void Awake()
    {
        flashingTime = Random.value * flashSpeed;
    }
    private void Update()
    {
        // Position calculation for player
        enemyTargetIndicator.transform.position = target.position - (target.position - transform.position).normalized * enemyTargetSpaceBetweenPlayer;

        // Flashing
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

        Destroy(gameObject);
        EnemyManager.Instance.DecreaseEnemyNumber();
    }

    protected abstract void Attack();

    protected void AttackCheck()
    {
        if (nextAttackTime < Time.time)
        {
            Attack();
            nextAttackTime = Time.time + attackSpeed;
        }
    }
}
