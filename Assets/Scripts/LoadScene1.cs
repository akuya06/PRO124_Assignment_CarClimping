using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene1 : MonoBehaviour
{
    // Hàm này sẽ được gắn vào Button
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
