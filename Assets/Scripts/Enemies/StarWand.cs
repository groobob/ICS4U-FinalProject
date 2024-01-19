/*
 * Class to manage the ai and behaviour for the star with a wand enemy
 * 
 * @author Richard
 * @version January 17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro.EditorUtilities;

public class StarWand : Enemy
{
    // Values for the enemy
    [Header("Values")]
    [SerializeField] float spaceBetweenPlayerSquared;
    [SerializeField] float rangeSquared;
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
        if (distanceToPlayer < rangeSquared && timeElapsed > reloadTime)
        {
            ProjectileManager.Instance.SpawnProjectile(_rb.position, new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y).normalized * projectileSpeed, 0);
            timeElapsed = 0f;
        }
        if (distanceToPlayer > detectionRadiusSquared || distanceToPlayer < spaceBetweenPlayerSquared) return;
        if (path == null) return;
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
}
