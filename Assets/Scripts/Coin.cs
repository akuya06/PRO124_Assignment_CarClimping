using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    // Gán tag cho từng loại coin: "Coin5", "Coin25", "Coin100", "Coin500"
    public int coinValue = 5; // Giá trị mặc định, có thể chỉnh trong Inspector

    [Header("Collection Effect Settings")]
    public float moveUpDistance = 1f; // Khoảng cách coin bay lên
    public float effectDuration = 0.8f; // Thời gian hiệu ứng
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Đường cong chuyển động
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0); // Đường cong fade

    private bool isCollected = false;
    private SpriteRenderer spriteRenderer;

    public AudioClip collectionSound; // Âm thanh thu thập coin
    private void Start()
    {
        // Lấy component SpriteRenderer để điều khiển độ trong suốt
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Coin không có SpriteRenderer component!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Tăng số lượng coin cho player
            CoinManager.Instance.AddCoin(coinValue);

            // Phát âm thanh thu thập coin
            SoundFXManager.instance.PlaySound(collectionSound, transform, 1f);

            // Bắt đầu hiệu ứng thu thập
            StartCoroutine(CollectionEffect());
        }
    }

    private IEnumerator CollectionEffect()
    {
        // Vô hiệu hóa collider để tránh thu thập lại
        Collider2D coinCollider = GetComponent<Collider2D>();
        if (coinCollider != null)
        {
            coinCollider.enabled = false;
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * moveUpDistance;

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (elapsedTime < effectDuration)
        {
            float progress = elapsedTime / effectDuration;

            // Di chuyển coin lên trên theo đường cong
            if (moveCurve != null)
            {
                float moveProgress = moveCurve.Evaluate(progress);
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            }

            // Fade out coin theo đường cong
            if (spriteRenderer != null)
            {
                float fadeProgress = fadeCurve != null ? fadeCurve.Evaluate(progress) : (1f - progress);
                Color newColor = originalColor;
                newColor.a = fadeProgress;
                spriteRenderer.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo coin hoàn toàn trong suốt và ở vị trí cuối
        if (spriteRenderer != null)
        {
            Color finalColor = originalColor;
            finalColor.a = 0f;
            spriteRenderer.color = finalColor;
        }
        transform.position = targetPosition;

        // Xóa coin sau khi hiệu ứng hoàn thành
        Destroy(gameObject);
    }
}