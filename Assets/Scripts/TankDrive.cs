using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDrive : MonoBehaviour
{
    [Header("Tank Components")]
    [SerializeField] private Rigidbody2D tankBody; // Tank body rigidbody

    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 100f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer = -1;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private Transform[] groundCheckPoints;

    [Header("UI Controls")]
    [SerializeField] private Button moveForwardButton;
    [SerializeField] private Button moveBackwardButton;

    [Header("Audio")]
    [SerializeField] private AudioSource engineAudio;

    // Private variables
    private float moveInput;
    private bool isGrounded = false;

    // Input states for UI buttons
    private bool isForwardPressed = false;
    private bool isBackwardPressed = false;

    void Start()
    {
        if (tankBody == null)
            tankBody = GetComponent<Rigidbody2D>();

        SetupUIButtons();
    }

    void SetupUIButtons()
    {
        if (moveForwardButton != null)
        {
            moveForwardButton.onClick.AddListener(() => OnMoveForwardPressed());
        }

        if (moveBackwardButton != null)
        {
            moveBackwardButton.onClick.AddListener(() => OnMoveBackwardPressed());
        }
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
        HandleAudio();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        // Keyboard input - chỉ sử dụng W/S hoặc Up/Down Arrow
        bool keyForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool keyBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // Combine keyboard and UI input
        bool forward = keyForward || isForwardPressed;
        bool backward = keyBackward || isBackwardPressed;

        // Set movement input
        if (forward && !backward)
            moveInput = 1f;
        else if (!forward && backward)
            moveInput = -1f;
        else
            moveInput = 0f;
    }

    void HandleMovement()
    {
        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            // Di chuyển theo hướng phải của transform (horizontal movement trong 2D)
            Vector2 moveForce = transform.right * moveInput * moveSpeed * Time.fixedDeltaTime;
            tankBody.AddForce(moveForce);

            // Limit max speed
            if (tankBody.linearVelocity.magnitude > maxSpeed)
            {
                tankBody.linearVelocity = tankBody.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    void HandleAudio()
    {
        if (engineAudio != null)
        {
            bool isMoving = Mathf.Abs(moveInput) > 0.1f;

            if (isMoving && !engineAudio.isPlaying)
            {
                engineAudio.Play();
            }
            else if (!isMoving && engineAudio.isPlaying)
            {
                engineAudio.Stop();
            }
        }
    }

    void CheckGrounded()
    {
        isGrounded = false;

        if (groundCheckPoints == null || groundCheckPoints.Length == 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
            isGrounded = hit.collider != null;
        }
        else
        {
            foreach (Transform checkPoint in groundCheckPoints)
            {
                if (checkPoint != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, groundCheckDistance, groundLayer);
                    if (hit.collider != null)
                    {
                        isGrounded = true;
                        break;
                    }
                }
            }
        }
    }

    // UI Button Methods - chỉ giữ lại Forward/Backward
    public void OnMoveForwardPressed() { isForwardPressed = true; }
    public void OnMoveForwardReleased() { isForwardPressed = false; }
    public void OnMoveBackwardPressed() { isBackwardPressed = true; }
    public void OnMoveBackwardReleased() { isBackwardPressed = false; }

    // Public methods for other scripts
    public bool IsVehicleMoving()
    {
        return Mathf.Abs(moveInput) > 0.01f;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    // Visualize ground check in Scene view
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        if (groundCheckPoints != null && groundCheckPoints.Length > 0)
        {
            foreach (Transform checkPoint in groundCheckPoints)
            {
                if (checkPoint != null)
                {
                    Gizmos.DrawLine(checkPoint.position, checkPoint.position + Vector3.down * groundCheckDistance);
                }
            }
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}