using UnityEngine;
using UnityEngine.UI;

public class MenuMusicManager : MonoBehaviour
{
    public AudioSource menuMusicSource;
    public Slider musicSlider;

    void Start()
    {
        // Set slider giá trị ban đầu theo âm lượng hiện tại
        musicSlider.value = menuMusicSource.volume;

        // Lắng nghe sự thay đổi giá trị slider
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    void SetMusicVolume(float value)
    {
        menuMusicSource.volume = value;
    }
}
