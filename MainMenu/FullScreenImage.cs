using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class FullScreenImage : MonoBehaviour
{
    private bool isChanging = false;
    private CanvasGroup bgCanvasGroup;

    private bool isCounting = false;
    private float timeCounter = 0f;
    private int secondsPassed = 0;
    // Timer prende il tempo dal file. Quando chiude va a salvare, ma quando arriva a 10m o 20m fa un save
    private FileManager fileManager;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.Cancel.performed += ctx => CloseImageButton();
    }

    void OnEnable() { controls.Enable(); }
    void OnDisable() { controls.Disable(); }


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

        fileManager = FindFirstObjectByType<FileManager>();
        secondsPassed = fileManager.GetSecondsInFullScreenByWaifu(GetWaifuNameByOpenedImage());
    }

    public void CloseImageButton()
    {
        if (isCounting)
        {
            isCounting = false;
            secondsPassed += Mathf.FloorToInt(timeCounter);
            Debug.Log($"Tempo in fullscreen: {secondsPassed} secondi");
            timeCounter = 0f;
            fileManager.SetSecondsInFullScreenByWaifu(secondsPassed, GetWaifuNameByOpenedImage());
        }

        WaifuDetail waifuDetail = UnityEngine.Object.FindFirstObjectByType<WaifuDetail>();
        if (waifuDetail != null)
        {
            // Chiama la funzione CloseImage dal componente WaifuDetail
            waifuDetail.CloseImage();
        }
    }

    void Update()
    {
        if (bgCanvasGroup == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseImageButton();
        }

        if (Mathf.Approximately(bgCanvasGroup.alpha, 1f))
        {
            if (!isCounting)
            {
                isCounting = true;
                timeCounter = 0f;
            }

            timeCounter += Time.deltaTime;

            int totalSeconds = secondsPassed + Mathf.FloorToInt(timeCounter);

            if ((totalSeconds >= 600 && secondsPassed < 600) ||
                (totalSeconds >= 1200 && secondsPassed < 1200))
            {
                secondsPassed = totalSeconds;
                timeCounter = 0f;
                fileManager.SetSecondsInFullScreenByWaifu(secondsPassed, GetWaifuNameByOpenedImage());
                fileManager.SaveWaifuFile();
            }
        }

        if (!Mathf.Approximately(bgCanvasGroup.alpha, 1f))
        {
            isCounting = false;
        }

        if (Mathf.Approximately(bgCanvasGroup.alpha, 1f))
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryOpenPreviousImage();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryOpenNextImage();
            }
        }
    }

    public void TryOpenNextImage()
    {
        if (isChanging) return;
        isChanging = true;
        if (TryOpenNextHentaiImage()) { return; }

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
            if (nextIndex > maxUnlocked && nextIndex == 11) TryOpenNextHentaiImage(true);
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
        if(TryOpenPreviusHentaiImage()) { return; }

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

    private Waifu GetWaifuNameByOpenedImage()
    {
        string currentPath = PlayerPrefs.GetString("fullScreenPath");
        string[] pathParts = currentPath.Split('/');
        string waifuName = pathParts[2];
        string fileName = pathParts[3]; // es: Chiho_2
        string[] nameParts = fileName.Split('_');

        return (Waifu)System.Enum.Parse(typeof(Waifu), nameParts[0]);
    }

    private bool TryOpenNextHentaiImage(bool isFirst = false)
    {
        string path = PlayerPrefs.GetString("fullScreenPath");
        bool isMatch = Regex.IsMatch(path, @"_0_\d+$");
        if (isMatch || isFirst)
        {
            string lastChar = "0";
            if (!isFirst) lastChar = path[path.Length - 1].ToString();

            int num = int.Parse(lastChar);
            if (num < 3)
            {
                num += 1;
            }
            else
            {
                isChanging = false;
                return true;
            }

            WaifuPage waifuPage = FindFirstObjectByType<WaifuPage>();
            waifuPage.HentaiButton(num.ToString());
            isChanging = false;
            return true;
        }

        return false;
    }

    private bool TryOpenPreviusHentaiImage()
    {
        string path = PlayerPrefs.GetString("fullScreenPath");
        bool isMatch = Regex.IsMatch(path, @"_0_\d+$");
        if (isMatch)
        {
            string lastChar = path[path.Length - 1].ToString();
            int num = int.Parse(lastChar);
            if (num > 1)
            {
                num -= 1;
                WaifuPage waifuPage = FindFirstObjectByType<WaifuPage>();
                waifuPage.HentaiButton(num.ToString());
            }
            else
            {
                WaifuDetail waifuDetail = UnityEngine.Object.FindFirstObjectByType<WaifuDetail>();
                int firstUnderscore = path.IndexOf('_');
                string newPath = path.Substring(0, firstUnderscore) + "_10";
                waifuDetail.OpenImageAtPath(newPath);
            }
            
            isChanging = false;
            return true;
        }

        return false;
    }
}
