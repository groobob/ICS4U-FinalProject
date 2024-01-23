/*
 * Class to manage the ai and behaviour for the star enemy
 * 
 * @author Richard, Evan
 * @version January 15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Star : Enemy
{
    
    // References for the enemy
    [Header("References")]
    [SerializeField] private float nextWaypointDistance = 3f;

    // Pathfinding
    private Path path;
    private int currentWaypoint = 0;
    private float distanceToPlayer;

    // other references to own components
    private Seeker _seeker;
    private Animator _animator;

    private void Start()
    {
        // Component initialization
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        sprite = transform.GetChild(0).transform;

        // Constant path creation
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (dead) return;
        if (!checkDisabled()) return;
        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        if (distanceToPlayer > detectionRadiusSquared) return;
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * baseMoveSpeed * Time.deltaTime;

        _rb.AddForce(force, ForceMode2D.Force);

        float distance = Vector2.Distance(_rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        AttackCheck();
    }

    protected override void Death()
    {
        dead = true;
        _animator.Play("Star-Die");
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        Destroy(enemyTargetIndicator);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
    }

    protected override void Attack()
    {
        Collider2D[] hitbox = Physics2D.OverlapCircleAll(transform.position, meleeRange);

        foreach (Collider2D hit in hitbox)
        {
            PlayerStats target = hit.gameObject.GetComponent<PlayerStats>();
            if (target)
            {
                target.TakeDamage(1);
            }
        }
    }
}
