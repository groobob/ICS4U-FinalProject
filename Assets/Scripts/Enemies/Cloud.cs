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
    private bool charging = false;
    private float timeElapsed;

    // References for the enemy
    [Header("References")]
    [SerializeField] private float nextWaypointDistance = 3f;


    // Pathfinding
    private Path path;
    private int currentWaypoint = 0;
    private float distanceToPlayer;

    // other references to own components
    private Seeker _seeker;

    private new void Start()
    {
        // Component initialization
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

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

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!checkDisabled()) return;
        timeElapsed += Time.deltaTime;
        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        if (distanceToPlayer > detectionRadiusSquared || distanceToPlayer < spaceBetweenPlayerSquared) return;
        if (path == null) return;

        // Figure out another time
        // RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, target.position, distanceToPlayer + 1f, 6);
        // if hitInfo.collider != null && hitInfo.collider.gameObject.GetComponent<PlayerController>() != null
        if (distanceToPlayer < rangeSquared && timeElapsed > reloadTime)
        {
            Debug.Log("raycast worked");
            timeElapsed = 0f;
            charging = true;
        }

        if (currentWaypoint >= path.vectorPath.Count) return;

        if (!charging)
        {
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
            if (timeElapsed < chargeTime)
            {
                Vector2 direction = ((Vector2)target.position - _rb.position).normalized;
                charging = false;
                timeElapsed = 0f;
                //shoot
                Debug.Log("cloud attack");
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

    protected override void Attack() { }
}
