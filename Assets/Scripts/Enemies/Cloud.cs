/*
 * Class to manage the ai and behaviour for the cloud enemy
 * 
 * @author Richard
 * @version January 17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Cloud : Enemy
{
    // Values for the enemy
    [Header("Values")]
    [SerializeField] private float spaceBetweenPlayerSquared;
    [SerializeField] private float rangeSquared;
    [SerializeField] private float reloadTime;
    [SerializeField] private float chargeTime;
    [SerializeField] private float animationChargeTime;
    [SerializeField] private float animationAttackTime;
    [SerializeField] private float animationDeathTime;
    [SerializeField] private float projectileSpeed;
    private bool charging = false;
    private float timeElapsed;
    private bool attacking = false;
    private bool attacked = false;

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
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (dead) return;
        if (!checkDisabled())
        {
            attacking = false;
            attacked = false;
            charging = false;
            timeElapsed = 0f;
            _animator.Play("Cloud-Move");
            return;
        }
        timeElapsed += Time.deltaTime;
        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        if (distanceToPlayer > detectionRadiusSquared) return;
        if (path == null) return;
        // Figure out another time
        // RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, target.position, distanceToPlayer + 1f, 6);
        // if hitInfo.collider != null && hitInfo.collider.gameObject.GetComponent<PlayerController>() != null
        if (distanceToPlayer < rangeSquared && timeElapsed > reloadTime)
        {
            // Debug.Log("raycast worked");
            timeElapsed = 0f;
            charging = true;
        }

        if (currentWaypoint >= path.vectorPath.Count) return;

        if (!charging)
        {

            if (!attacked) _animator.Play("Cloud-Move");
            if (distanceToPlayer < spaceBetweenPlayerSquared) return;
            if (currentWaypoint >= path.vectorPath.Count) return;

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rb.position).normalized;
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
            if (attacking == false)
            {
                _animator.Play("Cloud-Charge");
                Invoke("PlayChargeLoop", animationChargeTime);
                attacking = true;
            }

            if (timeElapsed > chargeTime)
            {
                _animator.Play("Cloud-Attack");
                attacked = true;
                Invoke("EndAttackAnimation", animationAttackTime);
                Vector2 direction = ((Vector2)target.position - _rb.position).normalized;
                charging = false;
                attacking = false;
                timeElapsed = 0f;
                ProjectileManager.Instance.SpawnProjectile(_rb.position, new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y).normalized * projectileSpeed, 0);
            }
            else
            {
                //tracer for bullet
            }
        }

        if (_rb.velocity.x >= 0.01f)
        {
            sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_rb.velocity.y <= 0.01f)
        {
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void PlayChargeLoop()
    {
        _animator.Play("Cloud-Charging");
    }

    private void EndAttackAnimation()
    {
        attacked = false;
    }

    protected override void Death()
    {
        dead = true;
        _animator.Play("Cloud-Die");
        gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
        DropManager.Instance.SpawnDrop(transform);
        Destroy(enemyTargetIndicator);
        Destroy(gameObject, animationDeathTime);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 9;
    }

    protected override void Attack() { }
}
