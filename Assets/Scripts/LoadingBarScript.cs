using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Thêm thư viện cho TextMeshPro nếu sử dụng

public class LoadingBarScript : MonoBehaviour
{
    public Slider loadingBar; // Slider cho loading bar
    public TextMeshProUGUI percentageText; // Text để hiển thị phần trăm
    public string sceneToLoad = "SceneName"; // Tên scene muốn tải

    private void Start()
    {
        // Bắt đầu tải scene khi script bắt đầu chạy
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // Tạo một yêu cầu tải scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Không chuyển sang scene mới cho đến khi quá trình tải hoàn tất
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Cập nhật tiến trình tải
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            // Hiển thị phần trăm
            percentageText.text = Mathf.FloorToInt(progress * 100) + "%";

            // Nếu tải xong, chuyển sang scene mới
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}