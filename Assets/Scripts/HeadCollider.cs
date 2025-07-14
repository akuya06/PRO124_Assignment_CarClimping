using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    private Fuel fuelScript;

    private void Start()
    {
        // Find the Fuel script in the scene (adjust if needed)
        fuelScript = FindObjectOfType<Fuel>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (fuelScript != null)
            {
                fuelScript.SendMessage("GameOver");
            }
        }
    }
}