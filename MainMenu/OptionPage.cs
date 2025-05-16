using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class OptionPage : MonoBehaviour
{
    public Slider audioSlider;        // Slider per controllare l'audio
    public Toggle fullscreenToggle;   // Toggle per passare tra schermo intero e finestra
    public TMP_Dropdown resolutionDropdown;
    public TextMeshProUGUI percentage;
    public Canvas mainPageCanvas;
    public Canvas optionPageCanvas;

    private Resolution[] availableResolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private bool isFullScreen;

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

        isFullScreen = PlayerPrefs.GetInt("isFullScreen", 1) == 1;
        fullscreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;

        // Aggiungi listener per gli eventi di modifica
        audioSlider.onValueChanged.AddListener(OnVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);

        SetUpResolution();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    private void SetUpResolution()
    {
        // Risoluzioni
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++) {
            Resolution res = availableResolutions[i];
            // Evita risoluzioni duplicate
            if (!filteredResolutions.Exists(r => r.width == res.width && r.height == res.height)) {
                filteredResolutions.Add(res);
                string option = res.width + " x " + res.height;
                options.Add(option);

                if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = filteredResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Applica la risoluzione salvata
        Resolution savedResolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(savedResolution.width, savedResolution.height, isFullScreen);
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

        // Reimposta la risoluzione per applicare correttamente la modalità fullscreen
        Resolution current = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(current.width, current.height, isFullscreen);
    }

    public void OnResolutionChanged(int index)
    {
        Resolution selectedResolution = filteredResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void ReturnButton()
    {
        optionPageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);
    }
}
