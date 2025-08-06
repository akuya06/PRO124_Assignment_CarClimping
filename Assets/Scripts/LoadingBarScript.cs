using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingBarScript : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingBar; // Slider cho loading bar
    public TextMeshProUGUI percentageText; // Text để hiển thị phần trăm
    
    [Header("Scene Settings")]
    public string sceneToLoad = "SceneName"; // Tên scene muốn tải
    
    [Header("Loading Settings")]
    public float minimumLoadingTime = 2f; // Thời gian tải tối thiểu (để người dùng thấy loading bar)
    
    private void Start()
    {
        // Kiểm tra null references
        if (loadingBar == null)
        {
            Debug.LogError("Loading Bar chưa được gán trong Inspector!");
            return;
        }
        
        if (percentageText == null)
        {
            Debug.LogError("Percentage Text chưa được gán trong Inspector!");
            return;
        }
        
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene name chưa được nhập!");
            return;
        }
        
        // Bắt đầu tải scene
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        float startTime = Time.time;
        
        // Tạo một yêu cầu tải scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        if (operation == null)
        {
            Debug.LogError($"Không thể tải scene: {sceneToLoad}. Kiểm tra tên scene và Build Settings!");
            yield break;
        }

        // Không chuyển sang scene mới cho đến khi quá trình tải hoàn tất
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Cập nhật tiến trình tải (Unity báo cáo tối đa 0.9f khi allowSceneActivation = false)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Đảm bảo loading bar hiển thị trong thời gian tối thiểu
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < minimumLoadingTime)
            {
                float timeProgress = elapsedTime / minimumLoadingTime;
                progress = Mathf.Min(progress, timeProgress);
            }
            
            // Cập nhật UI
            loadingBar.value = progress;
            percentageText.text = Mathf.FloorToInt(progress * 100) + "%";

            // Nếu tải xong và đã đủ thời gian tối thiểu, chuyển sang scene mới
            if (operation.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
            {
                // Hiển thị 100% trước khi chuyển scene
                loadingBar.value = 1f;
                percentageText.text = "100%";
                
                yield return new WaitForSeconds(0.1f); // Delay nhỏ để người dùng thấy 100%
                
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}