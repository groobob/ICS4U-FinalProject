using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 direction;
    PlayerCamera cam;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Define RigidBody
        cam = FindObjectOfType<PlayerCamera>();
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Get Direction of Player Movement
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * runSpeed; // Make the player move
    }
}
