using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTire;
    [SerializeField] private Rigidbody2D backTire;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D carRb;

    private float moveInput;

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Vector2 frontTireVelocity = frontTire.velocity;
        Vector2 backTireVelocity = backTire.velocity;
        frontTireVelocity.x = moveInput * speed;
        backTireVelocity.x = moveInput * speed;
        frontTire.velocity = frontTireVelocity;
        backTire.velocity = backTireVelocity;
    }
}
