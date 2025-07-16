using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public Rigidbody2D carRb;
    public RectTransform needle;
    public float maxSpeed = 100f;
    public float minRotation = -180f; // Góc quay khi tốc độ = 0
    public float maxRotation = 0f;    // Góc quay khi tốc độ max

    public bool flipNeedle = false; // Bật/tắt lật kim

    void Update()
    {
        float speed = carRb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);
        float rotationZ = Mathf.Lerp(minRotation, maxRotation, t);
        needle.localRotation = Quaternion.Euler(0f, 0f, rotationZ);

        // Lật ngược sprite kim bằng scale Y
        if (flipNeedle)
            needle.localScale = new Vector3(needle.localScale.x, -Mathf.Abs(needle.localScale.y), needle.localScale.z);
        else
            needle.localScale = new Vector3(needle.localScale.x, Mathf.Abs(needle.localScale.y), needle.localScale.z);
    }
}