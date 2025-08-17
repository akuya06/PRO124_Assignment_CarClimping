using UnityEngine;
using TMPro; // Thêm nếu dùng TextMeshPro

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    public int totalCoins = 0;
    public TMP_Text coinText; // Gán TextMeshPro UI trong Inspector

    private const string CoinKey = "TotalCoins";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            LoadCoin();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        SaveCoin();
        UpdateCoinUI();
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = totalCoins.ToString();
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(CoinKey, totalCoins);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        totalCoins = PlayerPrefs.GetInt(CoinKey, 0);
    }
}