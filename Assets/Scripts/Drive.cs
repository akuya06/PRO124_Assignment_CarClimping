using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTire;
    [SerializeField] private Rigidbody2D backTire;
    [SerializeField] private float speed = 150f;
    [SerializeField] private Rigidbody2D carRb;

    // UI Buttons (assign in Inspector)
    [SerializeField] private Button brakeButton;
    [SerializeField] private Button gasButton;

    // Win UI or logic (assign in Inspector if needed)
    [SerializeField] private GameObject winPanel;

    private float moveInput;
    private bool hasWon = false;
    private bool isGasPressed = false;
    private bool isBrakePressed = false;

    public bool IsVehicleMoving()
    {
        return Mathf.Abs(moveInput) > 0.01f;
    }

    public void OnGasButtonPressed()
    {
        isGasPressed = true;
    }

    public void OnBrakeButtonPressed()
    {
        isBrakePressed = true;
    }

    public void OnButtonReleased()
    {
        isGasPressed = false;
        isBrakePressed = false;
    }

    void Update()
    {
        if (hasWon) return;

        // Xử lý logic Gas/Brake cho cả phím và nút UI
        bool gas = isGasPressed || Input.GetKey(KeyCode.A);
        bool brake = isBrakePressed || Input.GetKey(KeyCode.D);

        if (gas && !brake)
            moveInput = 1f;
        else if (!gas && brake)
            moveInput = -1f;
        else
            moveInput = 0f;

        // Debug.Log("moveInput: " + moveInput);
    }

    void FixedUpdate()
    {
        if (hasWon) return;
        frontTire.AddTorque(moveInput * speed * Time.fixedDeltaTime);
        backTire.AddTorque(moveInput * speed * Time.fixedDeltaTime);
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