using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDistanceText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private Transform _playerTrans;

    private Vector2 _startPosition;
    private float _bestDistance = 0f;
    private float _currentDistance = 0f;

    private const string BestDistanceKey = "BestDistance";

    private void Start()
    {
        _startPosition = _playerTrans.position;
        LoadBestDistance();
    }

    private void Update()
    {
        Vector2 distance = (Vector2)_playerTrans.position - _startPosition;
        distance.y = 0f;

        if (distance.x < 0)
        {
            distance.x = 0;
        }

        _currentDistance = distance.x;

        // Cập nhật best distance nếu đạt kỷ lục mới
        if (_currentDistance > _bestDistance)
        {
            _bestDistance = _currentDistance;
            SaveBestDistance();
        }

        // Hiển thị: "100m - best: 500m"
        _distanceText.text = _currentDistance.ToString("F0") + "m (best: " + _bestDistance.ToString("F0") + "m)";
    }

    private void SaveBestDistance()
    {
        PlayerPrefs.SetFloat(BestDistanceKey, _bestDistance);
        PlayerPrefs.Save();
    }

    private void LoadBestDistance()
    {
        _bestDistance = PlayerPrefs.GetFloat(BestDistanceKey, 0f);
    }

    // Method để reset best distance (có thể gọi từ menu settings)
    public void ResetBestDistance()
    {
        _bestDistance = 0f;
        PlayerPrefs.DeleteKey(BestDistanceKey);
        PlayerPrefs.Save();
    }

    // Method để lấy current distance (có thể dùng cho GameOver screen)
    public float GetCurrentDistance()
    {
        return _currentDistance;
    }

    // Method để lấy best distance (có thể dùng cho GameOver screen)
    public float GetBestDistance()
    {
        return _bestDistance;
    }
}