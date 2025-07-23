using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        // Your game over logic here
        Debug.Log("Game Over!");
    }
}