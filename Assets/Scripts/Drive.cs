using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D frontTire;
    [SerializeField] private Rigidbody2D backTire;
    [SerializeField] public float speed = 150f;
    [SerializeField] private Rigidbody2D carRb;
    public float airControlStrength = 2.0f; // Tăng giá trị này để xe xoay mạnh hơn

    // Ground check variables
    [SerializeField] private LayerMask groundLayer = -1; // Layer mask cho ground
    [SerializeField] private float groundCheckDistance = 1.5f; // Khoảng cách check ground
    [SerializeField] private Transform[] groundCheckPoints; // Các điểm check ground (front tire, back tire)
    
    // UI Buttons (assign in Inspector)
    [SerializeField] private Button brakeButton;
    [SerializeField] private Button gasButton;

    // Win UI or logic (assign in Inspector if needed)
    [SerializeField] private GameObject winPanel;

    private float moveInput;
    private bool hasWon = false;
    private bool isGasPressed = false;
    private bool isBrakePressed = false;
    private bool isGrounded = false;

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

        // Check ground status
        CheckGrounded();

        // Debug.Log("moveInput: " + moveInput);
    }

    void FixedUpdate()
    {
        if (hasWon) return;

        // Luôn cho phép điều khiển bánh xe
        frontTire.AddTorque(moveInput * speed * Time.fixedDeltaTime);
        backTire.AddTorque(moveInput * speed * Time.fixedDeltaTime);

        // CHỈ cho phép điều khiển xoay thân xe khi xe KHÔNG chạm đất (ở trên không)
        if (!isGrounded)
        {
            carRb.AddTorque(-moveInput * speed * airControlStrength * Time.fixedDeltaTime);
        }
    }

    private void CheckGrounded()
    {
        isGrounded = false;

        // Nếu không có ground check points được set, dùng raycast từ vị trí xe
        if (groundCheckPoints == null || groundCheckPoints.Length == 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
            isGrounded = hit.collider != null;
        }
        else
        {
            // Check từ các điểm được chỉ định (ví dụ: vị trí bánh xe)
            foreach (Transform checkPoint in groundCheckPoints)
            {
                if (checkPoint != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, groundCheckDistance, groundLayer);
                    if (hit.collider != null)
                    {
                        isGrounded = true;
                        break; // Chỉ cần một điểm chạm đất là đủ
                    }
                }
            }
        }
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

    // Visualize ground check trong Scene view
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Màu xanh khi chạm đất, đỏ khi trên không
            Gizmos.color = isGrounded ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        // Vẽ raycast từ các ground check points
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
            // Vẽ raycast từ vị trí xe nếu không có check points
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}