/*
 * Class to manage the ai and behaviour for the shark enemy
 * 
 * @author Richard
 * @version January 17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Shark : Enemy
{
    // Values for the enemy
    [Header("Values")]
    [SerializeField] private float fleeRangeSquared;
    [SerializeField] private float reloadTime;
    [SerializeField] private float attackCharge;
    [SerializeField] private float projectileSpeed;
    private float timeElapsed;
    private bool attacking;

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
        EndlagEntity(Random.Range(1.0f, 5.0f));
    }

    private void UpdatePath()
    {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
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
        timeElapsed += Time.deltaTime;
        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        
        if (path == null) return;
        if (timeElapsed > reloadTime)
        {
            attacking = true;
            timeElapsed = 0f;
            _animator.Play("Shark-Shoot");
            return;
        }
        if (timeElapsed > attackCharge && attacking == true)
        {
            SoundManager.Instance.PlayAudio(11, gameObject.GetComponent<AudioSource>());
            ProjectileManager.Instance.SpawnProjectileSpread(_rb.position, new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y).normalized * projectileSpeed, 0, 11, 2 * Mathf.PI / 11);
            timeElapsed = 0f;
            attacking = false;
        }

        if (!attacking)
        {
            if (distanceToPlayer < fleeRangeSquared)
            {
                if (currentWaypoint >= path.vectorPath.Count) return;

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rb.position).normalized;
                direction = new Vector2(-direction.x, -direction.y);
                Vector2 force = direction * baseMoveSpeed * Time.deltaTime;

                _rb.AddForce(force, ForceMode2D.Force);

                float distance = Vector2.Distance(_rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                // roam
            }

            if (_rb.velocity.x >= 0.01f)
            {
                _animator.Play("Shark-Move");
                sprite.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (_rb.velocity.x <= -0.01f)
            {
                _animator.Play("Shark-Move");
                sprite.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                _animator.Play("Shark-Idle");
            }
        }
    }

    protected override void Death()
    {
        dead = true;
        _animator.Play("Shark-Die");
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        Destroy(enemyTargetIndicator);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 9;
    }

    protected override void Attack() { }
}
