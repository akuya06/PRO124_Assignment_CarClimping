using UnityEngine;

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
}