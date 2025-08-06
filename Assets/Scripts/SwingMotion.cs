using UnityEngine;
using UnityEngine.SceneManagement; // Thêm để load lại scene hoặc chuyển scene

public class SwingMotion : MonoBehaviour
{
    public float speed = 2.0f;  // Tốc độ lắc
    public float angle = 30.0f; // Góc lắc tối đa (độ)
    public GameObject gameOverPanel; // Kéo GameOverPanel từ Inspector vào đây

    private Vector3 initialRotation;

    void Start()
    {
        // Lưu lại góc quay ban đầu của đối tượng
        initialRotation = transform.rotation.eulerAngles;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Ẩn panel khi bắt đầu
    }

    void Update()
    {
        // Tính toán góc lắc sử dụng hàm sine
        float swingAngle = Mathf.Sin(Time.time * speed) * angle;

        // Áp dụng góc quay mới vào đối tượng
        transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, initialRotation.z + swingAngle);
    }

    // Xử lý va chạm với nhân vật
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head"))
        {
            Debug.Log("Collision with Head detected!");
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
            else
                Debug.LogWarning("GameOverPanel is not assigned!");
        }
    }
}