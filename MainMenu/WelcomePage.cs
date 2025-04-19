using UnityEngine;
using System;

public class WelcomePage : MonoBehaviour
{
    public Canvas welcomePageCanvas;
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    public Canvas rulesPageCanvas;
    public Canvas optionPageCanvas;
    public Canvas idleCanvas;

    private void Awake() {
        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(false);
        rulesPageCanvas.gameObject.SetActive(false);
        optionPageCanvas.gameObject.SetActive(false);
        idleCanvas.gameObject.SetActive(false);

        float oldVolume = PlayerPrefs.GetFloat("audioVolume", 0f);
        if (oldVolume != 0f) {
            AudioListener.volume = oldVolume;
        }

        if (PlayerPrefs.GetInt("skipWelcomePage") == 1) {
            PlayerPrefs.SetInt("skipWelcomePage", 0);
            StartButton();
        }
    }

    public void StartButton()
    {
        mainPageCanvas.gameObject.SetActive(true);
        idleCanvas.gameObject.SetActive(true);
        Destroy(welcomePageCanvas.gameObject);
    }
}
