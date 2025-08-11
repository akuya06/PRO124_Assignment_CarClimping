using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingBarScript : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingBar; // Slider cho loading bar
    public TextMeshProUGUI percentageText; // Text để hiển thị phần trăm
    public GameObject loadingScreen; // Loading screen GameObject để tắt

    [Header("Loading Settings")]
    public float loadingDuration = 3f; // Thời gian loading (giả lập)
    public float delayBeforeHiding = 0.5f; // Thời gian delay trước khi tắt loading screen

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

        if (loadingScreen == null)
        {
            Debug.LogWarning("Loading Screen GameObject chưa được gán trong Inspector!");
        }

        // Đảm bảo loading screen được bật lúc bắt đầu
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Bắt đầu quá trình loading (KHÔNG load scene)
        StartCoroutine(SimulateLoadingOnly());
    }

    private IEnumerator SimulateLoadingOnly()
    {
        float startTime = Time.time;
        float progress = 0f;

        // Reset UI
        loadingBar.value = 0f;
        percentageText.text = "0%";


        // Simulate loading progress
        while (progress < 1f)
        {
            // Tính tiến trình dựa trên thời gian
            float elapsedTime = Time.time - startTime;
            progress = Mathf.Clamp01(elapsedTime / loadingDuration);

            // Cập nhật UI
            loadingBar.value = progress;
            percentageText.text = Mathf.FloorToInt(progress * 100) + "%";

            yield return null;
        }

        // Đảm bảo hiển thị 100%
        loadingBar.value = 1f;
        percentageText.text = "100%";


        // Delay để người dùng thấy 100%
        yield return new WaitForSeconds(delayBeforeHiding);

        // Tắt loading screen (KHÔNG chuyển scene)
        HideLoadingScreen();
    }

    /// <summary>
    /// Tắt loading screen
    /// </summary>
    public void HideLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
        else
        {
            // Nếu không có loading screen được gán, tắt GameObject này
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Hiển thị loading screen
    /// </summary>
    public void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Restart loading process (CHỈ restart loading bar, không load scene)
    /// </summary>
    public void RestartLoading()
    {
        StopAllCoroutines();
        ShowLoadingScreen();
        StartCoroutine(SimulateLoadingOnly());
    }
}