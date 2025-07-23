using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    public Transform playerHead;
    public float spawnHeight = 0.5f; // Sát ngay trên đầu

    void SpawnCrate()
    {
        if (cratePrefab == null)
        {
            Debug.LogError("cratePrefab chưa được gán!");
            return;
        }
        Vector3 spawnPosition = playerHead.position + Vector3.up * spawnHeight;
        Debug.Log($"SpawnCrate called! Spawn position: {spawnPosition}");
        Instantiate(cratePrefab, spawnPosition, Quaternion.identity);
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnCrate), 2f, 5f);
    }
}