using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided object is the player's head
        if (collision.gameObject.CompareTag("Head"))
        {
            // Trigger game over logic
            GameManager.Instance.GameOver();
            Debug.Log("Game Over! Player hit a trap.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
