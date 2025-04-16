using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionPage : MonoBehaviour
{
    public Slider audioSlider;        // Slider per controllare l'audio
    public Toggle fullscreenToggle;   // Toggle per passare tra schermo intero e finestra
    public TextMeshProUGUI percentage;
    public Canvas mainPageCanvas;
    public Canvas optionPageCanvas;

    private void Awake()
    {
        // Inizializza i valori iniziali per il volume e la modalità schermo
        float oldVolume = PlayerPrefs.GetFloat("audioVolume", 0f);
        if (oldVolume != 0f) {
            audioSlider.value = oldVolume;
            AudioListener.volume = oldVolume;
            UpdateVolumeText(oldVolume);
        } else {
            audioSlider.value = AudioListener.volume;
        }

        bool isFullScreen = PlayerPrefs.GetInt("isFullScreen", 1) == 1;
        fullscreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;

        // Aggiungi listener per gli eventi di modifica
        audioSlider.onValueChanged.AddListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    // Funzione per cambiare il volume
    public void OnVolumeChanged(float value)
    {
        UpdateVolumeText(value);
        PlayerPrefs.SetFloat("audioVolume", value);
        PlayerPrefs.Save();
        AudioListener.volume = value;
    }

    public void UpdateVolumeText(float value)
    {
        percentage.text = (Math.Truncate(value * 100)).ToString() + "%";
    }

    // Funzione per cambiare la modalità schermo (full screen o finestra)
    public void OnFullscreenChanged(bool isFullscreen)
    {
        PlayerPrefs.SetInt("isFullScreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        Screen.fullScreen = isFullscreen;
    }

    public void ReturnButton()
    {
        optionPageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);
    }
}
