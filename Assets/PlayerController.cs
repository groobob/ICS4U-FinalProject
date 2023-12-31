using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 direction;
    private PlayerCamera _cameraScript;
    //Mouse Info
    private Vector3 mousePos;
    private Vector3 mousePlayerVector;

    //Weapon
    [SerializeField] private Transform _weaponPos;
    private float weaponDisplacement = 1f;
    private float weaponAngle = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Define RigidBody
        _cameraScript = FindObjectOfType<PlayerCamera>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GetMouseInfo();
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Get Direction of Player Movement
        _rb.velocity = direction * runSpeed; // Make the player 
        AnimateWeapon();
    }

    public void UpdateWeapon(float displacement, float angleOffset)
    {
        weaponDisplacement = displacement;
        weaponAngle = angleOffset;
    }

    private void GetMouseInfo()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        mousePlayerVector = (mousePos - transform.position).normalized;
        //Debug.Log(mousePos + " " + mousePlayerVector);
    }
    private void AnimateWeapon()
    {
        float angle = -1 * Mathf.Atan2(mousePlayerVector.y, mousePlayerVector.x) * Mathf.Rad2Deg;
        _weaponPos.rotation = Quaternion.AngleAxis(angle + weaponAngle, Vector3.back); 
        Vector3 weaponOffset = mousePlayerVector * weaponDisplacement;
        _weaponPos.position = transform.position + weaponOffset;
    }
}
