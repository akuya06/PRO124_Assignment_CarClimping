using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    void Update()
    {
        // Bấm phím R để restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRestartClicked();
        }
    }

    public void OnRestartClicked()
    {
        //Time.timeScale = 1f; // Reset time scale trước khi load scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        //Time.timeScale = 1f; // Reset time scale trước khi load scene
        SceneManager.LoadScene("Menu");
    }
}
