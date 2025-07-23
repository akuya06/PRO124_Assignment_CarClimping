using UnityEngine;
using UnityEngine.SceneManagement;

public class NutThoatTrongBangTamDung : MonoBehaviour
{
      // Để sử dụng chức năng SceneManagement



    // Hàm này sẽ được gọi khi button được bấm
    public void LoadScene(string sceneName)
    {
        // Tải scene theo tên
        SceneManager.LoadScene(sceneName);
    }
}

