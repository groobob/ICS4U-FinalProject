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
    [SerializeField] float fleeRangeSquared;
    [SerializeField] float movementSpeed;
    [SerializeField] float reloadTime;
    [SerializeField] float projectileSpeed;
    float timeElapsed;

    // References for the enemy
    [Header("References")]
    [SerializeField] float nextWaypointDistance = 3f;


    // Pathfinding
    Path path;
    int currentWaypoint = 0;
    float distanceToPlayer;

    // other references to own components
    Seeker _seeker;
    Rigidbody2D _rb;

    public Shark(int HP) : base(HP)
    {

    }

    void Start()
    {
        // Component initialization
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        sprite = transform.GetChild(0).transform;

        // Constant path creation
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        
        if (path == null) return;
        if (timeElapsed > reloadTime)
        {
            ProjectileManager.Instance.SpawnProjectileSpread(_rb.position, new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y).normalized * projectileSpeed, 0, 11, 2 * Mathf.PI / 11);
            timeElapsed = 0f;
        }

        if (distanceToPlayer < fleeRangeSquared)
        {
            if (currentWaypoint >= path.vectorPath.Count) return;

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rb.position).normalized;
            direction = new Vector2(-direction.x, -direction.y);
            Vector2 force = direction * movementSpeed * Time.deltaTime;

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
            sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_rb.velocity.y <= 0.01f)
        {
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
