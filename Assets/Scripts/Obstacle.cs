using UnityEngine;

public class Obstavle : MonoBehaviour
{
    public GameObject obstaclePrefab; // Gán prefab chướng ngại vật trong Inspector
    public Transform player; // Gán transform của xe/nhân vật
    public float spawnDistance = 100f; // Quãng đường mỗi lần sinh chướng ngại vật
    public float spawnHeight = 10f; // Độ cao xuất hiện chướng ngại vật
    public float minXOffset = 3f; // Khoảng lệch trái/phải so với xe
    public float maxXOffset = 6f;

    private float lastSpawnDistance = 0f;

    void Update()
    {
        float traveled = player.position.x;
        if (traveled - lastSpawnDistance >= spawnDistance)
        {
            SpawnObstacle();
            lastSpawnDistance = traveled;
        }
    }

    void SpawnObstacle()
    {
        float xOffset = Random.Range(minXOffset, maxXOffset) * (Random.value > 0.5f ? 1 : -1);
        Vector3 spawnPos = new Vector3(player.position.x + xOffset, player.position.y + spawnHeight, 0f);
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
