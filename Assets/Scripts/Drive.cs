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

    // Win UI or logic (assign in Inspector if needed)
    [SerializeField] private GameObject winPanel;

    private bool hasWon = false;

    public bool IsVehicleMoving()
    {
        return Mathf.Abs(moveInput) > 0.01f; // Example logic to determine if the vehicle is moving
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWon) return;
        moveInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (hasWon) return;
        Vector2 frontTireVelocity = frontTire.linearVelocity;
        Vector2 backTireVelocity = backTire.linearVelocity;
        frontTireVelocity.x = moveInput * speed;
        backTireVelocity.x = moveInput * speed;
        frontTire.linearVelocity = frontTireVelocity;
        backTire.linearVelocity = backTireVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasWon) return;

        if (other.CompareTag("Finish"))
        {
            Win();
        }
    }

    private void Win()
    {
        hasWon = true;
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        Time.timeScale = 0f; // Pause the game
        // Optionally stop the car
        if (carRb != null)
        {
            carRb.linearVelocity = Vector2.zero;
            carRb.angularVelocity = 0f;
        }
        // Add more win logic here if needed
    }
}
