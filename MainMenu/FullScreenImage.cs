using UnityEngine;
using System;

public class FullScreenImage : MonoBehaviour
{
    private bool isChanging = false;
    private CanvasGroup bgCanvasGroup;

    private void Start()
    {
        Transform bgTransform = transform.Find("BackGroundGradient");
        if (bgTransform != null)
        {
            bgCanvasGroup = bgTransform.GetComponent<CanvasGroup>();
            if (bgCanvasGroup == null)
            {
                bgCanvasGroup = bgTransform.gameObject.AddComponent<CanvasGroup>();
            }
        }
        else
        {
            Debug.LogError("[FullScreenImage.cs] BackGroundGradient non trovato come figlio.");
        }
    }

    public void CloseImageButton()
    {
        WaifuDetail waifuDetail = UnityEngine.Object.FindFirstObjectByType<WaifuDetail>();
        if (waifuDetail != null)
        {
            // Chiama la funzione CloseImage dal componente WaifuDetail
            waifuDetail.CloseImage();
        }
        else
        {
            Debug.LogError("[FullScreenImage.cs] WaifuDetail non trovato nella scena!");
        }
    }

    void Update()
    {
        if (bgCanvasGroup == null || !Mathf.Approximately(bgCanvasGroup.alpha, 1f))
        return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            TryOpenPreviousImage();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryOpenNextImage();
        }
    }

    public void TryOpenNextImage()
    {
        if (isChanging) return;
        isChanging = true;

        try
        {
            WaifuDetail waifuDetail = UnityEngine.Object.FindFirstObjectByType<WaifuDetail>();
            FileManager fileManager = UnityEngine.Object.FindFirstObjectByType<FileManager>();
            if (waifuDetail == null || fileManager == null)
            {
                Debug.LogError("[FullScreenImage.cs] WaifuDetail o FileManager non trovato!");
                return;
            }

            string currentPath = PlayerPrefs.GetString("fullScreenPath");
            string[] pathParts = currentPath.Split('/');
            if (pathParts.Length < 4) return;

            string waifuName = pathParts[2];
            string fileName = pathParts[3]; // es: Chiho_2
            string[] nameParts = fileName.Split('_');
            if (nameParts.Length < 2) return;

            int currentIndex = int.Parse(nameParts[1]);
            int nextIndex = currentIndex + 1;

            Waifu activeWaifu = (Waifu)Enum.Parse(typeof(Waifu), waifuName);
            int maxUnlocked = fileManager.GetImageStepByWaifu(activeWaifu);
            if (nextIndex > maxUnlocked) return;

            string nextImagePath = $"Texture/Waifu/{waifuName}/{waifuName}_{nextIndex}";
            if (Resources.Load<Texture2D>(nextImagePath) != null)
            {
                waifuDetail.OpenImageAtPath(nextImagePath);
            }
        }
        finally
        {
            isChanging = false;
        }
    }

    public void TryOpenPreviousImage()
    {
        if (isChanging) return;
        isChanging = true;

        try
        {
            WaifuDetail waifuDetail = UnityEngine.Object.FindFirstObjectByType<WaifuDetail>();
            if (waifuDetail == null)
            {
                Debug.LogError("[FullScreenImage.cs] WaifuDetail non trovato!");
                return;
            }

            string currentPath = PlayerPrefs.GetString("fullScreenPath");
            string[] pathParts = currentPath.Split('/');
            if (pathParts.Length < 4) return;

            string waifuName = pathParts[2];
            string fileName = pathParts[3]; // es: Chiho_2
            string[] nameParts = fileName.Split('_');
            if (nameParts.Length < 2) return;

            int currentIndex = int.Parse(nameParts[1]);
            int previousIndex = currentIndex - 1;

            if (previousIndex < 1) return; // Non esiste l'immagine precedente

            string previousImagePath = $"Texture/Waifu/{waifuName}/{waifuName}_{previousIndex}";
            if (Resources.Load<Texture2D>(previousImagePath) != null)
            {
                waifuDetail.OpenImageAtPath(previousImagePath);
            }
        }
        finally
        {
            isChanging = false;
        }
    }
}
