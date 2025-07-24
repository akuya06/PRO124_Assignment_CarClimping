using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pauseImage; // Gán Image pause game trong Inspector
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    public float slideDuration = 0.3f; // Thời gian hiệu ứng slide

    private RectTransform pauseRect;
    private Vector2 hiddenPos;
    private Vector2 shownPos;
    private Coroutine slideCoroutine;

    private void Start()
    {
        pauseRect = pauseImage.GetComponent<RectTransform>();
        shownPos = pauseRect.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0, pauseRect.rect.height); // Ẩn lên trên

        pauseRect.anchoredPosition = hiddenPos;
        pauseImage.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseImage != null)
        {
            pauseImage.SetActive(true);
            StartSlide(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseImage != null)
        {
            StartSlide(false);
        }
    }

    private void StartSlide(bool slideDown)
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlidePauseImage(slideDown));
    }

    private System.Collections.IEnumerator SlidePauseImage(bool slideDown)
    {
        float elapsed = 0f;
        Vector2 start = slideDown ? hiddenPos : shownPos;
        Vector2 end = slideDown ? shownPos : hiddenPos;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            pauseRect.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
        pauseRect.anchoredPosition = end;
        if (!slideDown)
            pauseImage.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}