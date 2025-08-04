using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CoinPrefab
{
    public GameObject prefab;
    public int value; // 5, 25, 100, 500
    [Range(0f, 100f)]
    public float spawnRate = 25f; // Tỉ lệ xuất hiện (%)
}

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefabs")]
    public List<CoinPrefab> coinPrefabs = new List<CoinPrefab>();

    [Header("Spawn Settings")]
    public Transform player; // Gán transform của xe/nhân vật
    public float spawnInterval = 10f; // Thời gian giữa các lần spawn (giây)
    public int minCoinsPerSpawn = 4; // Số coin tối thiểu mỗi lần spawn
    public int maxCoinsPerSpawn = 6; // Số coin tối đa mỗi lần spawn

    [Header("Line Formation Settings")]
    public float coinSpacing = 2f; // Khoảng cách giữa các coin trong hàng
    public bool randomizeLineDirection = true; // Có ngẫu nhiên hướng hàng không
    public float lineHeight = 3f; // Độ cao của hàng coin so với ground

    [Header("Ground Detection")]
    public LayerMask groundLayer = 1; // Layer của ground
    public float raycastDistance = 50f; // Khoảng cách raycast xuống dưới
    public float heightAboveGround = 1f; // Độ cao coin spawn trên ground

    [Header("Spawn Area")]
    public float minXOffset = -5f; // Khoảng cách trái so với player
    public float maxXOffset = 15f; // Khoảng cách phải so với player
    public float forwardOffset = 20f; // Spawn phía trước player

    private float lastSpawnTime = 0f;

    void Start()
    {
        // Thiết lập giá trị mặc định cho coin prefabs nếu chưa có
        SetupDefaultCoinPrefabs();
    }

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnCoinsInLine();
            lastSpawnTime = Time.time;
        }
    }

    void SetupDefaultCoinPrefabs()
    {
        if (coinPrefabs.Count == 0)
        {
            // Tạo danh sách mặc định nếu chưa có prefab nào
            for (int i = 0; i < 4; i++)
            {
                coinPrefabs.Add(new CoinPrefab());
            }
        }

        // Thiết lập giá trị mặc định
        if (coinPrefabs.Count >= 1)
        {
            coinPrefabs[0].value = 5;
            coinPrefabs[0].spawnRate = 50f; // 50% tỉ lệ xuất hiện
        }
        if (coinPrefabs.Count >= 2)
        {
            coinPrefabs[1].value = 25;
            coinPrefabs[1].spawnRate = 30f; // 30%
        }
        if (coinPrefabs.Count >= 3)
        {
            coinPrefabs[2].value = 100;
            coinPrefabs[2].spawnRate = 15f; // 15%
        }
        if (coinPrefabs.Count >= 4)
        {
            coinPrefabs[3].value = 500;
            coinPrefabs[3].spawnRate = 5f; // 5%
        }
    }

    void SpawnCoinsInLine()
    {
        if (player == null) return;

        int coinCount = Random.Range(minCoinsPerSpawn, maxCoinsPerSpawn + 1);

        // Xác định vị trí bắt đầu của hàng coin
        float startX = player.position.x + forwardOffset + Random.Range(minXOffset, maxXOffset);
        
        // Xác định hướng của hàng (trái sang phải hoặc phải sang trái)
        float direction = randomizeLineDirection && Random.value > 0.5f ? -1f : 1f;
        
        // Tìm ground tại vị trí bắt đầu
        Vector3 startPosition = new Vector3(startX, player.position.y + 10f, 0f);
        Vector3 groundPosition = FindGroundPosition(startPosition);
        
        if (groundPosition != Vector3.zero)
        {
            // Spawn các coin theo hàng ngang
            for (int i = 0; i < coinCount; i++)
            {
                // Tính vị trí cho coin thứ i
                float coinX = startX + (i * coinSpacing * direction);
                Vector3 coinSpawnPos = new Vector3(coinX, player.position.y + 10f, 0f);
                
                // Tìm ground tại vị trí coin này
                Vector3 coinGroundPos = FindGroundPosition(coinSpawnPos);
                
                if (coinGroundPos != Vector3.zero)
                {
                    // Chọn prefab coin
                    GameObject selectedPrefab = SelectCoinPrefab();
                    if (selectedPrefab != null)
                    {
                        // Spawn coin tại vị trí đã tính
                        Vector3 finalPosition = new Vector3(
                            coinGroundPos.x,
                            coinGroundPos.y + heightAboveGround + lineHeight,
                            coinGroundPos.z
                        );

                        GameObject coin = Instantiate(selectedPrefab, finalPosition, Quaternion.identity);
                        
                        // Set giá trị coin nếu có script Coin
                        Coin coinScript = coin.GetComponent<Coin>();
                        if (coinScript != null)
                        {
                            CoinPrefab coinPrefabData = GetCoinPrefabData(selectedPrefab);
                            if (coinPrefabData != null)
                            {
                                coinScript.coinValue = coinPrefabData.value;
                            }
                        }
                    }
                }
            }
        }
    }

    void SpawnCoins()
    {
        if (player == null) return;

        int coinCount = Random.Range(minCoinsPerSpawn, maxCoinsPerSpawn + 1);

        for (int i = 0; i < coinCount; i++)
        {
            SpawnSingleCoin();
        }
    }

    void SpawnSingleCoin()
    {
        // Chọn vị trí spawn ngẫu nhiên
        float xOffset = Random.Range(minXOffset, maxXOffset);
        Vector3 spawnPosition = new Vector3(
            player.position.x + forwardOffset + xOffset,
            player.position.y + 10f, // Bắt đầu từ trên cao
            0f
        );

        // Tìm ground bằng raycast
        Vector3 groundPosition = FindGroundPosition(spawnPosition);
        if (groundPosition != Vector3.zero)
        {
            // Chọn prefab coin ngẫu nhiên dựa trên tỉ lệ
            GameObject selectedPrefab = SelectCoinPrefab();
            if (selectedPrefab != null)
            {
                // Spawn coin trên ground
                Vector3 finalPosition = new Vector3(
                    groundPosition.x,
                    groundPosition.y + heightAboveGround,
                    groundPosition.z
                );

                GameObject coin = Instantiate(selectedPrefab, finalPosition, Quaternion.identity);
                
                // Set giá trị coin nếu có script Coin
                Coin coinScript = coin.GetComponent<Coin>();
                if (coinScript != null)
                {
                    CoinPrefab coinPrefabData = GetCoinPrefabData(selectedPrefab);
                    if (coinPrefabData != null)
                    {
                        coinScript.coinValue = coinPrefabData.value;
                    }
                }
            }
        }
    }

    Vector3 FindGroundPosition(Vector3 startPosition)
    {
        // Raycast xuống dưới để tìm ground
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, raycastDistance, groundLayer);
        
        if (hit.collider != null)
        {
            return hit.point;
        }
        
        return Vector3.zero; // Không tìm thấy ground
    }

    GameObject SelectCoinPrefab()
    {
        // Tính tổng tỉ lệ
        float totalRate = 0f;
        foreach (var coinPrefab in coinPrefabs)
        {
            if (coinPrefab.prefab != null)
                totalRate += coinPrefab.spawnRate;
        }

        if (totalRate <= 0f) return null;

        // Chọn ngẫu nhiên dựa trên tỉ lệ
        float randomValue = Random.Range(0f, totalRate);
        float currentRate = 0f;

        foreach (var coinPrefab in coinPrefabs)
        {
            if (coinPrefab.prefab != null)
            {
                currentRate += coinPrefab.spawnRate;
                if (randomValue <= currentRate)
                {
                    return coinPrefab.prefab;
                }
            }
        }

        // Fallback: trả về prefab đầu tiên nếu có
        foreach (var coinPrefab in coinPrefabs)
        {
            if (coinPrefab.prefab != null)
                return coinPrefab.prefab;
        }

        return null;
    }

    CoinPrefab GetCoinPrefabData(GameObject prefab)
    {
        foreach (var coinPrefab in coinPrefabs)
        {
            if (coinPrefab.prefab == prefab)
                return coinPrefab;
        }
        return null;
    }

    // Gizmos để visualize spawn area trong Scene view
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Vector3 spawnCenter = new Vector3(
            player.position.x + forwardOffset,
            player.position.y,
            0f
        );

        // Vẽ vùng spawn
        Vector3 leftBottom = new Vector3(spawnCenter.x + minXOffset, spawnCenter.y - 5f, 0f);
        Vector3 rightTop = new Vector3(spawnCenter.x + maxXOffset, spawnCenter.y + 5f, 0f);
        
        Gizmos.DrawWireCube(
            (leftBottom + rightTop) / 2f,
            new Vector3(maxXOffset - minXOffset, 10f, 1f)
        );

        // Vẽ preview hàng coin
        Gizmos.color = Color.cyan;
        float previewStartX = spawnCenter.x;
        for (int i = 0; i < maxCoinsPerSpawn; i++)
        {
            Vector3 coinPos = new Vector3(previewStartX + (i * coinSpacing), spawnCenter.y + lineHeight, 0f);
            Gizmos.DrawWireSphere(coinPos, 0.5f);
        }
    }
}