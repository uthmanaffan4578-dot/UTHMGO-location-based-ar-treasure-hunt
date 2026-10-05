using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MUSIC : MonoBehaviour
{
    public Slider VolumeSlider;

    private static MUSIC instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Bunuh duplicate kalau dah ada instance
        }
    }

    void Start()
    {
        // Load volume dari PlayerPrefs dan apply ke AudioListener
        LoadVolume();

        // First time run – kalau VolumeSlider memang dah diset di inspector
        if (VolumeSlider != null)
        {
            VolumeSlider.value = PlayerPrefs.GetFloat("soundVolume", 1f);
            VolumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuba cari balik slider baru (kalau ada) dalam scene yang baru
        GameObject sliderObj = GameObject.Find("musicSlider");
        if (sliderObj != null)
        {
            VolumeSlider = sliderObj.GetComponent<Slider>();

            VolumeSlider.value = PlayerPrefs.GetFloat("soundVolume", 1f);
            VolumeSlider.onValueChanged.RemoveAllListeners(); // clear dulu
            VolumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        }
        else
        {
            VolumeSlider = null; // Untuk elakkan pointer ke slider lama
        }
    }

    public void SetVolume()
    {
        if (VolumeSlider != null)
        {
            AudioListener.volume = VolumeSlider.value;
            SaveVolume();
        }
    }

    public void SaveVolume()
    {
        if (VolumeSlider != null)
            PlayerPrefs.SetFloat("soundVolume", VolumeSlider.value);
    }

    public void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("soundVolume", 1f);
        AudioListener.volume = volume;

        if (VolumeSlider != null)
            VolumeSlider.value = volume;
    }
}