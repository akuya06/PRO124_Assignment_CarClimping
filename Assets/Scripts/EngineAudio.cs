using UnityEngine;

public class EngineAudioController : MonoBehaviour
{
    public AudioSource idleSource;
    public AudioSource accelerationSource;
    public AudioSource constantSource;

    public Rigidbody2D carRigidbody;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    void Start()
    {
        idleSource.loop = true;
        accelerationSource.loop = true;
        constantSource.loop = true;

        idleSource.Play();
        accelerationSource.Play();
        constantSource.Play();
    }

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        float t = Mathf.InverseLerp(0, 100, speed); // 0 ~ 100 km/h

        // Volume blending
        idleSource.volume = 1f - t;
        accelerationSource.volume = Mathf.Clamp01(1f - Mathf.Abs(t - 0.5f) * 2f);
        constantSource.volume = t;

        // Pitch control
        float pitch = Mathf.Lerp(minPitch, maxPitch, t);
        idleSource.pitch = pitch;
        accelerationSource.pitch = pitch;
        constantSource.pitch = pitch;
    }
}
