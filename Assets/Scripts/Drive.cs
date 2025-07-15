using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTire;
    [SerializeField] private Rigidbody2D backTire;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D carRb;

    // UI Buttons (assign in Inspector)
    [SerializeField] private Button brakeButton;
    [SerializeField] private Button gasButton;

    // Win UI or logic (assign in Inspector if needed)
    [SerializeField] private GameObject winPanel;

    private float moveInput;
    private bool hasWon = false;

    public bool IsVehicleMoving()
    {
        return Mathf.Abs(moveInput) > 0.01f;
    }

    void Update()
    {
        if (hasWon) return;

        // Simulate button press/release for Brake (A) and Gas (D)
        if (brakeButton != null)
        {
            var brakePointer = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            if (Input.GetKeyDown(KeyCode.A))
                brakeButton.OnPointerDown(brakePointer);
            if (Input.GetKeyUp(KeyCode.A))
                brakeButton.OnPointerUp(brakePointer);
        }
        if (gasButton != null)
        {
            var gasPointer = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            if (Input.GetKeyDown(KeyCode.D))
                gasButton.OnPointerDown(gasPointer);
            if (Input.GetKeyUp(KeyCode.D))
                gasButton.OnPointerUp(gasPointer);
        }

        // Optional: set moveInput for car movement
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
        Time.timeScale = 0f;
        if (carRb != null)
        {
            carRb.linearVelocity = Vector2.zero;
            carRb.angularVelocity = 0f;
        }
    }
}