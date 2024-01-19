/*
 * Script for controlling the player.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject upgrades;
    //Player Movement Related
    [Header("Player Movement")]
    [SerializeField] private float runSpeed;
    private Rigidbody2D _rb;
    private Vector2 direction;

    //Camera Script
    private PlayerCamera _cameraScript;
    //Mouse Info
    private Vector3 mousePos;
    private Vector3 mousePlayerVector;

    //Player Stats
    [SerializeField] private PlayerStats _playerStats;

    //Weapon
    [SerializeField] private Transform _weaponPos;
    private Weapons currentWeapon;
    private float weaponDisplacement = 1f;
    private float weaponAngle = 0f;
    public int numOfAttacks; // USED FOR ON ATTACK UPGRADES

    // Tempo Attack
    [Header("Tempo Related")]
    private float tempoCDTime;
    [SerializeField] private float tempoAttackCD;
    [SerializeField] private float tempoCost;
    [SerializeField] private float tempoRequirement;

    private float nextAttackTime = 0;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Define RigidBody
        _cameraScript = FindObjectOfType<PlayerCamera>();
        currentWeapon = gameObject.AddComponent<StarterSword>();
        currentWeapon.SetPlayer(this);
        runSpeed = _playerStats.GetMoveSpeed();
        numOfAttacks = 0;
    }

    private void Update()
    {
        GetMouseInfo();
        AnimateWeapon();
        MainAttack();
        TempoAttack();
        runSpeed = _playerStats.GetMoveSpeed();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    /**
     * Method to call when changing the weapon of a Player. Deletes the previous weapon and adds a new one.
     * @param weaponType The class of the weapon to swap to.
     */
    public void UpdateWeapon(System.Type weaponType)
    {
        Destroy(currentWeapon);
        currentWeapon = gameObject.AddComponent(weaponType) as Weapons;
        currentWeapon.SetPlayer(this);
        weaponDisplacement = currentWeapon.GetWeaponDisplacement();
        weaponAngle = currentWeapon.GetWeaponAngle();
    }
    /**
     * Method for returning a Vector2 of the current weapon's position in space.
     * @return Vector2
     */
    public Vector2 GetRealWeaponPosition()
    {
        return _weaponPos.position;
    }
    /**
     * Method for returning a Quaternion of the current weapon.
     * @return Quaternion
     */
    public Quaternion GetRealWeaponAngle()
    {
        return _weaponPos.rotation;
    }
    /**
     * Method for checking/attacking.
     */
    private void MainAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (nextAttackTime <= Time.time)
            {
                nextAttackTime = Time.time + currentWeapon.getReloadTime();
                currentWeapon.Attack(); // call the attack method on the weapon

                numOfAttacks+= 1;
                upgradeAttacks();
                Debug.Log("attack");
            }
        }
    }

    private void upgradeAttacks()
    {
        foreach (OnAttackUpgrades upgrade in upgrades.GetComponents<OnAttackUpgrades>())
        {
            upgrade.upgradeAttack();
        }
    }

    /**
     * Method for setting the variables for the mouse's info
     */
    private void GetMouseInfo()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mousePlayerVector = (mousePos - transform.position).normalized;
        //Debug.Log(mousePos + " " + mousePlayerVector);
    }

    private void TempoAttack()
    {
        if (Input.GetKeyDown(KeyCode.F) && tempoCDTime < Time.time)
        {
            if (_playerStats.tempo >= tempoRequirement && _playerStats.SpendTempo(tempoCost))
            {
                tempoCDTime = Time.time + tempoAttackCD;
                Debug.Log("Tempo Attack");
            }
            else
            {
                Debug.Log("Not enough tempo");
            }
        }
    }

    /**
     * Method for doing movement.
     */
    private void Movement()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Get Direction of Player Movement
        direction.Normalize(); // Fixes diagonal directions going faster than intended
        _rb.velocity = direction * runSpeed * ApplySpeedMods();
    }
    /**
     * Method for animating the weapon alongside the mouse.
     */
    private void AnimateWeapon()
    {
        float angle = -1 * Mathf.Atan2(mousePlayerVector.y, mousePlayerVector.x) * Mathf.Rad2Deg;
        _weaponPos.rotation = Quaternion.AngleAxis(angle + weaponAngle, Vector3.back); 
        Vector3 weaponOffset = mousePlayerVector * weaponDisplacement;
        _weaponPos.position = transform.position + weaponOffset;
    }

    private float ApplySpeedMods()
    {
        float speedMultiplier = 1;
        speedMultiplier *= (1 + _playerStats.tempo / 350);
        Debug.Log(speedMultiplier);
        return speedMultiplier;
    }
}
