/*
 * Class to manage the ai and behaviour for the star enemy
 * 
 * @author Richard
 * @version January 15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Star : MonoBehaviour
{
    // Values for the enemy
    [Header("Values")]
    [SerializeField] float detectionRadius;
    [SerializeField] float movementSpeed;
    
    // References for the enemy
    [Header("References")]
    [SerializeField] Transform target;
    [SerializeField] Transform sprite;
    [SerializeField] float nextWaypointDistance = 3f;

    // Pathfinding
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float distanceToPlayer;

    // other references to own components
    Seeker _seeker;
    Rigidbody2D _rb;

    void Start()
    {
        // Component initialization
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        // Constant path creation
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceToPlayer = Vector2.Distance(_rb.position, target.position);
        if (distanceToPlayer > detectionRadius) return;
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime;

        _rb.AddForce(force, ForceMode2D.Force);

        float distance = Vector2.Distance(_rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
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
