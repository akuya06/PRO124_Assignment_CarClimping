using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Gán trong Inspector hoặc tự động tìm
    [SerializeField] private float moveSpeed = 5f;      // Tốc độ di chuyển bot
    [SerializeField] private float transparentDistance = 1f; // Khoảng cách để bot trong suốt

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerRb;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Lấy Rigidbody2D của người chơi
        if (playerTransform != null)
            playerRb = playerTransform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Chỉ di chuyển khi người chơi đang di chuyển
        if (playerRb != null && Mathf.Abs(playerRb.linearVelocity.x) > 0.01f)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Xử lý trong suốt khi ở gần người chơi
        if (playerTransform != null && spriteRenderer != null)
        {
            float distanceToPlayer = Mathf.Abs(transform.position.x - playerTransform.position.x);
            Color color = spriteRenderer.color;
            color.a = distanceToPlayer < transparentDistance ? 0.5f : 1f;
            spriteRenderer.color = color;
        }
    }
}