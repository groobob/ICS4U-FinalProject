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
using UnityEngine.UIElements.Experimental;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private float nextAttackTime = 0;

    //Secondary Weapon
    private SecondaryAttack secondaryAttack;
    private float nextSecondaryAttackTime = 0;

    // Tempo Attack
    [Header("Tempo Related")]
    public float tempoCDTime;
    [SerializeField] private float tempoAttackCD;
    [SerializeField] private float tempoCost;
    [SerializeField] private float tempoRequirement;
    private float tempoAttackCastTime = 0.8f;
    private float tempoSpellFinishTime;
    [SerializeField] private bool tempoFired = true;

    // Rush Attack
    [Header("Rush Related")]
    private float rushCDTime;
    [SerializeField] private float rushAttackCD;
    [SerializeField] private float rushCost;
    [SerializeField] private float rushRequirement;
    [SerializeField] private float rushVelocity;
    [SerializeField] private int rushDamage;
    [SerializeField] private float rushDuration;
    private float rushRadius = 2f;
    private float rushStun = 2f;
    private List<Enemy> rushHitEnemies;
    private float rushEndTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Define RigidBody
        _cameraScript = FindObjectOfType<PlayerCamera>();
        //currentWeapon = gameObject.AddComponent<StarterSword>();
        //currentWeapon.SetPlayer(this);
        UpdateWeapon(typeof(StarterSword)); //
        //secondaryAttack = gameObject.AddComponent<Sidegun>();
        //secondaryAttack.SetPlayer(this);
        UpdateSecondaryWeapon(typeof(FireColumn));//Sidegun
        runSpeed = _playerStats.GetMoveSpeed();
        numOfAttacks = 0;
    }

    private void Update()
    {
        GetMouseInfo();
        AnimateWeapon();
        MainAttack();
        SecondaryAttack();
        TempoAttack();
        RushAttack();
        runSpeed = _playerStats.GetMoveSpeed();
    }
    private void FixedUpdate()
    {
        if (_playerStats.GetRootReleaseTime() > Time.time)
        {
            return;
        }
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
     * Method to call when changing the secondary weapon of a Player. Deletes the previous weapon and adds a new one.
     * @param weaponType The class of the weapon to swap to.
     */
    public void UpdateSecondaryWeapon(System.Type weaponType)
    {
        Destroy(secondaryAttack);
        secondaryAttack = gameObject.AddComponent(weaponType) as SecondaryAttack;
        secondaryAttack.SetPlayer(this);
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
     * Method for returning the current weapon.
     * @return Weapons
     */
    public Weapons GetWeapon()
    {
        return currentWeapon;
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
            if (nextAttackTime <= Time.time && _playerStats.checkDisabled()) 
            {
                nextAttackTime = Time.time + currentWeapon.GetReloadTime();
                currentWeapon.Attack(); // call the attack method on the weapon
                Debug.Log("attack");

                numOfAttacks+= 1;
                upgradeAttacks();
                //Debug.Log("attack");
            }
        }
    }

    private void SecondaryAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (nextSecondaryAttackTime <= Time.time && _playerStats.checkDisabled())
            {
                _playerStats.EndlagEntity(0.6f);
                _playerStats.SpeedBoost(0.5f, 0.6f);
                nextSecondaryAttackTime = Time.time + secondaryAttack.GetReloadTime();
                secondaryAttack.Attack();
                numOfAttacks++;
                secondaryAttackUpgrades();
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

    private void secondaryAttackUpgrades()
    {
        foreach (OnAttackUpgrades upgrade in upgrades.GetComponents<OnAttackUpgrades>())
        {
            if (upgrade.GetType() == typeof(OwlSlice))
            {
                upgrade.attack();
            }
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

                if (PlayerManager.Instance.GetUpgradesPart().GetComponent<Earthquake>())
                {
                    PlayerManager.Instance.GetUpgradesPart().GetComponent<Earthquake>().attackEffect();
                }


                tempoFired = false;
                tempoCDTime = Time.time + tempoAttackCD;
                tempoSpellFinishTime = tempoAttackCastTime + Time.time;
                //Debug.Log("Tempo Attack");
                _playerStats.EndlagEntity(tempoAttackCastTime);
                _playerStats.SpeedBoost(0.2f, tempoAttackCastTime);
            }
            else
            {
                //Debug.Log("Not enough tempo");
            }
        }
        if ((tempoSpellFinishTime - tempoAttackCastTime*2/3) < Time.time && !tempoFired)
        {
            tempoFired = true;
            ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 25, 1);
        }

    }

    private void RushAttack()
    {
        if (rushEndTime > Time.time)
        {
            //Debug.Log("rush hitbox exists");
            Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, rushRadius);
            foreach (Collider2D c in hitBox)
            {
                Enemy e = c.gameObject.GetComponent<Enemy>();
                bool ignore = false;
                if (e)
                {
                    foreach (Enemy check in rushHitEnemies)
                    {
                        //Debug.Log(check);
                        if (check == e)
                        {
                            ignore = true;
                            break;
                        }
                    }
                    if (!ignore)
                    {
                        //Debug.Log("Damage Done");
                        rushHitEnemies.Add(e);
                        e.TakeDamage(rushDamage);
                        e.StunEntity(rushStun);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && rushCDTime < Time.time)
        {
            if (_playerStats.tempo >= rushRequirement && _playerStats.SpendTempo(rushCost))
            {
                rushCDTime = Time.time + rushAttackCD;
                //Debug.Log("rush Attack");
                Vector3 rushMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                rushMousePos.z = transform.position.z;
                Vector3 rushMousePlayerVector = (mousePos - transform.position).normalized;
                _playerStats.RootEntity(rushDuration);
                _rb.velocity = rushMousePlayerVector * rushVelocity;
                rushEndTime = Time.time + rushDuration;
                rushHitEnemies = new List<Enemy>();
            }
            else
            {
                //Debug.Log("Not enough tempo for rush");
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
        _rb.velocity = direction * runSpeed * ApplySpeedModsPlayer();
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

    public float ApplySpeedModsPlayer()
    {
        float speedMultiplier = 1;
        speedMultiplier *= (1 + _playerStats.tempo / 400);
        speedMultiplier *= (_playerStats.speedFactor);
        return speedMultiplier;
    }
}
