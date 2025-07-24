using UnityEngine;

public class Coin : MonoBehaviour
{
    // Gán tag cho từng loại coin: "Coin5", "Coin25", "Coin100", "Coin500"
    public int coinValue = 5; // Giá trị mặc định, có thể chỉnh trong Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tăng số lượng coin cho player
            CoinManager.Instance.AddCoin(coinValue);
            Destroy(gameObject);
        }
    }
}