using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Kontrol edilecek AudioSource
    public Slider volumeSlider;     // UI Slider

    private const string VolumeKey = "volume";

    void Awake()
    {
        // Eğer bu script sahneler arasında kalıcı olacaksa:
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.75f); // Varsayılan %75
        audioSource.volume = savedVolume;
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void OnVolumeChanged(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
}