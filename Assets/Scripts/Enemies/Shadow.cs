/*
 * Class to manage the ai and behaviour for the shadow enemy
 * 
 * @author Richard
 * @version January 17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Shadow : Enemy
{
    // Values for the enemy
    [Header("Values")]
    [SerializeField] private float rangeSquared;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float chargeTime;
    [SerializeField] private float chargeDuration;
    private float timeElapsed;
    private bool charging = false;
    private Vector2 chargeDirection;

    [SerializeField] private float knockbackStrength = 15f;
    [SerializeField] private float knockbackDuration = 0.3f;

    // References for the enemy
    [Header("References")]
    [SerializeField] private float nextWaypointDistance = 3f;

    // Pathfinding
    private Path path;
    private int currentWaypoint = 0;
    private float distanceToPlayer;

    // other references to own components
    private Seeker _seeker;

    private bool previousChargingValue;

    private void Start()
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
        if (!checkDisabled() && !charging) return;

        distanceToPlayer = Vector2.SqrMagnitude(new Vector2(target.position.x - _rb.position.x, target.position.y - _rb.position.y));
        if (distanceToPlayer > detectionRadiusSquared) return;
        if (path == null) return;

        if (distanceToPlayer < rangeSquared)
        {
            //Attack stuff
            charging = true;
            if (!previousChargingValue)
            {
                SoundManager.Instance.PlayAudio(11, gameObject.GetComponent<AudioSource>());
            }
            previousChargingValue = true;


        }

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
            timeElapsed += Time.deltaTime;
            if (timeElapsed < chargeTime)
            {
                chargeDirection = ((Vector2) target.position - _rb.position).normalized;
            }
            else
            {
                _rb.velocity = chargeDirection * dashSpeed * (timeElapsed - chargeTime) / 100;
            }

            if (timeElapsed > chargeTime + chargeDuration)
            {
                charging = false;
                previousChargingValue = false;
                timeElapsed = 0f;
            }

            AttackCheck();
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

    protected override void Attack()
    {
        Collider2D[] hitbox = Physics2D.OverlapCircleAll(transform.position, meleeRange);

        foreach (Collider2D hit in hitbox)
        {
            PlayerStats target = hit.gameObject.GetComponent<PlayerStats>();
            if (target)
            {
                if (target.TakeDamage(1))
                {
                    target.RootEntity(knockbackDuration);
                    target.GiveKnockBack(gameObject, knockbackStrength, knockbackDuration);
                }
            }
        }
    }
}
