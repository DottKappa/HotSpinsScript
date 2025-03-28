using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class WaifuDetail : MonoBehaviour
{
    public Button openImageButton; // Riferimento al pulsante per aprire l'immagine
    private Image fullScreenImage; // Riferimento all'immagine che si aprirà a tutto schermo
    private CanvasGroup canvasGroup; // Riferimento al CanvasGroup per il fade (componente dell'immagine)

    public float fadeDuration = 1.0f; // Durata del fade in/out
    private bool isButtonEnabled; // Flag per abilitare o meno il bottone
    private string buttonImagePath; // Percorso dell'immagine per il bottone
    private string fullScreenImagePath;

    void Awake()
    {
        GameObject fullScreenImageObject = GameObject.Find("FullScreenImage");
        if (fullScreenImageObject != null) {
            fullScreenImage = fullScreenImageObject.transform.Find("Image")?.GetComponent<Image>();
        } else {
            Debug.LogError("[WaifuDetail.cs] Non ho trovato l'elemento FullScreenImage");
        }

    }

    // Metodo di inizializzazione personalizzato
    public void Initialize(string buttonImagePath, string fullScreenImagePath, bool isButtonEnabled)
    {
        this.buttonImagePath = buttonImagePath;
        this.isButtonEnabled = isButtonEnabled;
        this.fullScreenImagePath = fullScreenImagePath;

        // Carica l'immagine per il bottone
        if (openImageButton != null && !string.IsNullOrEmpty(buttonImagePath)) {
            Texture2D texture = LoadImage(buttonImagePath);
            if (texture != null) {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                openImageButton.GetComponent<Image>().sprite = sprite;
            }
        }

        // Disabilita il pulsante se necessario
        openImageButton.interactable = isButtonEnabled;

        // Aggiungi i listener solo se il bottone è abilitato
        if (isButtonEnabled) {
            openImageButton.onClick.AddListener(OpenImage);
        }
    }

    private void Start()
    {
        // Assicurati che l'immagine sia nascosta all'inizio
        canvasGroup = fullScreenImage.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // L'immagine parte trasparente
    }

    // Funzione per aprire l'immagine a tutto schermo con fade-in
    public void OpenImage()
    {
        UpdateFullScreenImage(fullScreenImagePath);
        fullScreenImage.gameObject.SetActive(true);  // Mostra l'immagine
        StartCoroutine(FadeIn());
    }

    // Funzione per chiudere l'immagine con fade-out
    public void CloseImage()
    {
        StartCoroutine(FadeOut());
    }

    // Coroutine per il fade in
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Assicurati che l'immagine sia completamente visibile
    }

    // Coroutine per il fade out
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f; // Assicurati che l'immagine sia completamente invisibile
        fullScreenImage.gameObject.SetActive(false);  // Mostra l'immagine
    }

    // Funzione per caricare l'immagine dal path sul bottone
    private Texture2D LoadImage(string path)
    {
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture != null) {
            return texture;
        } else {
            Debug.LogError("[WaifuDetail] File non trovato in Resources: " + path);
            return null;
        }
    }

    // Funzione per cambiare l'immagine di fullScreenImage
    private void UpdateFullScreenImage(string imagePath)
    {
        // Carica l'immagine dalla cartella Resources con il percorso relativo
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null) {
            // Crea uno sprite dalla texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // Assegna lo sprite a fullScreenImage
            fullScreenImage.sprite = sprite;
        } else {
            Debug.LogError("[WaifuDetail] Immagine fullScreen non trovata in Resources: " + imagePath);
        }
    }
}
