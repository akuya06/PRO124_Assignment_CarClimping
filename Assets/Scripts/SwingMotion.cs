using UnityEngine;
using UnityEngine.SceneManagement; // Thêm để load lại scene hoặc chuyển scene

public class SwingMotion : MonoBehaviour
{
    public float speed = 2.0f;  // Tốc độ lắc
    public float angle = 30.0f; // Góc lắc tối đa (độ)

    private Vector3 initialRotation;

    void Start()
    {
        // Lưu lại góc quay ban đầu của đối tượng
        initialRotation = transform.rotation.eulerAngles;
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
        if (other.CompareTag("Head")) // Đảm bảo nhân vật có tag là "Player"
        {
            // Thực hiện logic game thua, ví dụ load lại scene hiện tại
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Hoặc có thể gọi hàm GameOver() nếu bạn có hệ thống quản lý game
        }
    }
}