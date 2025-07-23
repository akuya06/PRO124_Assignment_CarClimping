using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Biến để lưu trữ trạng thái tạm dừng
    private bool isPaused = false;

    // Hàm này sẽ được gọi khi nhấn phím ESC
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Tiếp tục game
                ResumeGame();
            }
            else
            {
                // Dừng game và chuyển sang scene pause
                PauseGame();
            }
        }
    }

    // Hàm để dừng game và chuyển sang màn hình pause
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Dừng thời gian game
        SceneManager.LoadScene("TamDung", LoadSceneMode.Additive); // Tải màn hình pause (PauseScene)
    }

    // Hàm để tiếp tục game khi bấm "Tiếp tục"
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Tiếp tục thời gian game
        SceneManager.UnloadSceneAsync("TamDung"); // Tắt màn hình pause (PauseScene)
    }

    // Hàm để chơi lại từ đầu (reload lại game scene)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Đảm bảo game tiếp tục bình thường
        SceneManager.LoadScene("Map1"); // Chuyển về scene game ban đầu
    }

    // Hàm để thoát game (nếu cần)
    public void QuitGame()
    {
        Application.Quit();
    }
}