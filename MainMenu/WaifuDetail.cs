using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class WaifuDetail : MonoBehaviour
{
    public Button openImageButton;

    private Image fullScreenImage;
    private Image fullScreenBg;

    private CanvasGroup imageCanvasGroup;
    private CanvasGroup bgCanvasGroup;

    public float fadeDuration = 1.0f;

    private bool isButtonEnabled;
    private string buttonImagePath;
    private string fullScreenImagePath;

    void Awake()
    {
        GameObject fullScreenImageObject = GameObject.Find("FullScreenImage");
        if (fullScreenImageObject != null)
        {
            fullScreenImage = fullScreenImageObject.transform.Find("Image")?.GetComponent<Image>();
            fullScreenBg = fullScreenImageObject.transform.Find("BackGroundGradient")?.GetComponent<Image>();

            imageCanvasGroup = fullScreenImage.GetComponent<CanvasGroup>();
            bgCanvasGroup = fullScreenBg.GetComponent<CanvasGroup>();

            if (imageCanvasGroup == null)
                imageCanvasGroup = fullScreenImage.gameObject.AddComponent<CanvasGroup>();
            if (bgCanvasGroup == null)
                bgCanvasGroup = fullScreenBg.gameObject.AddComponent<CanvasGroup>();
        }
        else
        {
            Debug.LogError("[WaifuDetail.cs] Non ho trovato l'elemento FullScreenImage");
        }
    }

    public void Initialize(string buttonImagePath, string fullScreenImagePath, bool isButtonEnabled, bool needBlur = false)
    {
        this.buttonImagePath = buttonImagePath;
        this.isButtonEnabled = isButtonEnabled;
        this.fullScreenImagePath = fullScreenImagePath;

        if (openImageButton != null && !string.IsNullOrEmpty(buttonImagePath))
        {
            Texture2D texture = LoadImage(buttonImagePath);
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                openImageButton.GetComponent<Image>().sprite = sprite;
            }
            if (!needBlur)
            {
                RemoveComponentByName(openImageButton.gameObject, "UIEffect");
            }
        }

        openImageButton.interactable = isButtonEnabled;

        if (isButtonEnabled)
        {
            openImageButton.onClick.AddListener(OpenImage);
        }
    }

    private void Start()
    {
        imageCanvasGroup.alpha = 0f;
        bgCanvasGroup.alpha = 0f;

        fullScreenImage.gameObject.SetActive(false);
        fullScreenBg.gameObject.SetActive(false);
    }

    public void OpenImage()
    {
        UpdateFullScreenImage(fullScreenImagePath);
        fullScreenImage.gameObject.SetActive(true);
        fullScreenBg.gameObject.SetActive(true);

        List<CanvasGroup> targets = new List<CanvasGroup> { imageCanvasGroup, bgCanvasGroup };
        StartCoroutine(FadeCanvasGroups(targets, 0f, 1f, fadeDuration));
    }

    public void CloseImage()
    {
        List<CanvasGroup> targets = new List<CanvasGroup> { imageCanvasGroup, bgCanvasGroup };
        StartCoroutine(FadeCanvasGroups(targets, 1f, 0f, fadeDuration, disableAfterFade: true));
    }

    private IEnumerator FadeCanvasGroups(List<CanvasGroup> groups, float from, float to, float duration, bool disableAfterFade = false)
    {
        float elapsed = 0f;

        foreach (var group in groups)
            group.alpha = from;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            foreach (var group in groups)
                group.alpha = alpha;

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var group in groups)
            group.alpha = to;

        if (disableAfterFade)
        {
            fullScreenImage.gameObject.SetActive(false);
            fullScreenBg.gameObject.SetActive(false);
        }
    }

    private Texture2D LoadImage(string path)
    {
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture != null)
        {
            return texture;
        }
        else
        {
            Debug.LogError("[WaifuDetail] File non trovato in Resources: " + path);
            return null;
        }
    }

    private void UpdateFullScreenImage(string imagePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            fullScreenImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("[WaifuDetail] Immagine fullScreen non trovata in Resources: " + imagePath);
        }
    }

    private void RemoveComponentByName(GameObject target, string componentName)
    {
        Component[] allComponents = target.GetComponents<Component>();
        foreach (Component comp in allComponents)
        {
            if (comp.GetType().Name == componentName)
            {
                Destroy(comp);
            }
        }
    }
}
