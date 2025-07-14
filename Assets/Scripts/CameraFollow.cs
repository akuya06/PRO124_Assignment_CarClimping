using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The object for the camera to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Camera offset
    [SerializeField] private float smoothSpeed = 5f; // Smoothing speed

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}