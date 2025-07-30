using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float currentFuel = 100f;
    [SerializeField] private float fuelConsumptionRate = 5f; // units per second

    // Reference to your vehicle controller script
    [SerializeField] private Drive drive;

    // Reference to the UI Slider for fuel
    [SerializeField] private Slider fuelBar;

    // Game over UI or logic (assign in Inspector if needed)
    [SerializeField] private GameObject gameOverPanel;

    private bool isGameOver = false;

    void Start()
    {
        if (fuelBar != null)
        {
            fuelBar.maxValue = maxFuel;
            fuelBar.value = currentFuel;
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver) return;

        if (drive != null && drive.IsVehicleMoving())
        {
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        }

        // Update your fuel bar UI here
        if (fuelBar != null)
        {
            fuelBar.value = currentFuel;
        }

        // Check for out of fuel
        if (currentFuel <= 0f)
        {
            GameOver();
            Time.timeScale = 0f; // Stop the game when out of fuel
        }
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }

    // Call this method when fuel is collected
    public void AddFuel(float amount)
    {
        if (isGameOver) return;
        currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
        if (fuelBar != null)
        {
            fuelBar.value = currentFuel;
        }
    }

    // Detect collision with fuel pickup or ground
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Fuel"))
        {
            AddFuel(20f); // Increase fuel by 20 units (adjust as needed)
            Destroy(other.gameObject); // Remove the fuel pickup from the scene
        }
        else if (other.CompareTag("Ground"))
        {
            GameOver(); // Game over if body hits the ground
        }
        else if (other.CompareTag("Trap"))
        {
            GameOver(); // Game over if head hits the ground
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (drive != null)
        {
            drive.enabled = false; // Stop the vehicle
        }
        // Add more game over logic here if needed
    }
}
