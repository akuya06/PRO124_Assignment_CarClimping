using UnityEngine;
using UnityEngine.SceneManagement;  // Để sử dụng SceneManager

public class MapLevel1 : MonoBehaviour
{
    // Phương thức này phải public và có tham số kiểu string
    public void SelectMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
