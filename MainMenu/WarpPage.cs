using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using TMPro;

public class WarpPage : MonoBehaviour
{
    public GameObject welcomePage;
    private FileManager fileManager;
    private IdleFileManager idleFileManager;
    public CameraMainMenu cameraMainMenu;
    private SteamAchievement steamAchievement;

    [Header("Pull screen")]
    public GameObject pullScreen;
    public GameObject pullOne;
    public GameObject pullTen;
    public TextMeshProUGUI[] missingArtText;
    private List<string> filteredSteps = new();
    private Dictionary<Waifu, int> waifuUnlockCounts = new();
    private List<string> waifuToWriteFile = new();
    public TextMeshProUGUI AvailableWarps;
    public TextMeshProUGUI ErrorText;
    private int countTotalWarp = 0;

    [Header("Warp file")]
    private string folder = "dataFiles";
    private string nameFile = "warpData.txt";

    [Header("Warp tile")]
    public GameObject warpTileLoop;
    public GameObject warpTilePull;

    private void Awake()
    {
        foreach (Waifu w in Enum.GetValues(typeof(Waifu)))
        {
            waifuUnlockCounts[w] = 0;
        }
    }

    private void Start()
    {
        warpTileLoop.SetActive(true);
        steamAchievement = FindFirstObjectByType<SteamAchievement>();
    }

    private void OnEnable()
    {
        if (!filteredSteps.Any())
        {
            fileManager = FindFirstObjectByType<FileManager>();
            filteredSteps.Clear();
            // loop con tutti i nomid delle waifu + prendo i dati dal fileManager
            foreach (Waifu waifuName in Enum.GetValues(typeof(Waifu)))
            {
                if (fileManager.GetIsUnlockedByWaifu(waifuName))
                {
                    InitFilteredWaifuSteps(waifuName.ToString(), fileManager.GetImageStepByWaifu(waifuName), fileManager.GetPointsByWaifu(waifuName));
                }
            }

            string folderPath = Path.Combine(Application.persistentDataPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                string[] fileString = fileContent.Split(';');
                RemoveStepsByKeys(fileString);
                countTotalWarp = fileString.Length;
            }
            UpdateMissingArtTexts();
        }

        idleFileManager = FindFirstObjectByType<IdleFileManager>();
        AvailableWarps.text = idleFileManager.GetNumberOfUnlockableRoom().ToString();
    }

    private void OnDisable()
    {
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
        if (waifuToWriteFile.Count > 0)
        {
            string toAppend = string.Join(";", waifuToWriteFile) + ";";
            File.AppendAllText(filePath, toAppend);
            waifuToWriteFile.Clear();  // svuoto la lista dopo il salvataggio
        }

        // Pulizia sprite per liberare memoria
        ClearSpritesInGameObject(pullOne);
        ClearSpritesInGameObject(pullTen);
        countTotalWarp = 0;
        foreach (Waifu w in Enum.GetValues(typeof(Waifu)))
        {
            waifuUnlockCounts[w] = 0;
        }
        GameObject parentGO = ErrorText.gameObject.transform.parent?.gameObject;
        parentGO.SetActive(false);
    }

    public void PullBy(int numberOfPulls = 1)
    {
        try
        {
            if (filteredSteps.Count < numberOfPulls)
            {
                throw new InvalidOperationException("[WarpPage.cs] Not enoght art for pull. Availability: " + filteredSteps.Count);
            }
            switch (numberOfPulls)
            {
                case 1: idleFileManager.UseUnlockableRoom(numberOfPulls); break;
                case 10: idleFileManager.UseUnlockableRoom(numberOfPulls - 1); break;
            }
        }
        catch (System.Exception e)
        {
            GameObject parentGO = ErrorText.gameObject.transform.parent?.gameObject;
            parentGO.SetActive(true);
            string message = e.Message;
            int index = message.IndexOf(']');
            message = message.Substring(index + 1).Trim();
            ErrorText.text = message;
            StartCoroutine(FadeInThenOut(parentGO.GetComponent<CanvasGroup>()));
            return;
        }

        cameraMainMenu.WarpSound();
        warpTileLoop.SetActive(false);
        warpTilePull.SetActive(true);
        List<string> pulledWaifu = GetRandomSteps(numberOfPulls);
        RemoveStepsByKeys(pulledWaifu.ToArray());
        waifuToWriteFile.AddRange(pulledWaifu);
        pullScreen.SetActive(true);

        switch (numberOfPulls)
        {
            case 1:
                {
                    LoadSpritesFromWaifuList(pullOne, pulledWaifu);
                    pullOne.SetActive(true);
                    break;
                }
            case 10:
                {
                    LoadSpritesFromWaifuList(pullTen, pulledWaifu);
                    pullTen.SetActive(true);
                    break;
                }
        }

        UpdateMissingArtTexts();
        AvailableWarps.text = idleFileManager.GetNumberOfUnlockableRoom().ToString();
        CanvasGroup canvasGroup = pullScreen.GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn(canvasGroup, 1f));
        countTotalWarp += pulledWaifu.Count;
        steamAchievement.CheckPullAchievement(countTotalWarp);
    }

    public void ClosePullScreen()
    {
        warpTilePull.SetActive(false);
        warpTileLoop.SetActive(true);
        InvertCg(pullScreen.GetComponent<CanvasGroup>());
        pullOne.SetActive(false);
        pullTen.SetActive(false);
        pullScreen.SetActive(false);
    }

    public void ReturnButton()
    {
        gameObject.SetActive(false);
        welcomePage.SetActive(true);
    }

    private void InvertCg(CanvasGroup cg)
    {
        cg.alpha = cg.alpha == 0 ? 1 : 0;
        cg.interactable = !cg.interactable;
        cg.blocksRaycasts = !cg.blocksRaycasts;
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// Inizializza la lista con gli step validi per una waifu data.    
    private void InitFilteredWaifuSteps(string waifuName, int maxStep, int points)
    {
        foreach (string stepName in Enum.GetNames(typeof(WaifuSteps)))
        {
            if (stepName.StartsWith(waifuName + "_") &&
                TryGetStepNumber(stepName, out int stepNum) &&
                stepNum <= maxStep &&
                !filteredSteps.Contains(stepName))
            {
                filteredSteps.Add(stepName);
            }
        }

        // --- PrestigeSteps (solo se maxStep >= 10) ---
        if (maxStep >= 10)
        {
            foreach (string pStepName in Enum.GetNames(typeof(PrestigeSteps)))
            {
                if (pStepName.StartsWith(waifuName + "_") &&
                    (int)(PrestigeSteps)Enum.Parse(typeof(PrestigeSteps), pStepName) <= points &&
                    !filteredSteps.Contains(pStepName))
                {
                    filteredSteps.Add(pStepName);
                }
            }
        }
    }

    /// Restituisce X nomi casuali unici dalla lista.
    private List<string> GetRandomSteps(int count)
    {
        if (filteredSteps.Count == 0 || count <= 0)
            return new List<string>();

        System.Random rng = new();
        return filteredSteps.OrderBy(_ => rng.Next()).Take(count).ToList();
    }

    /// Rimuove gli step corrispondenti alle chiavi specificate.
    private void RemoveStepsByKeys(string[] keysToRemove)
    {
        if (keysToRemove == null || keysToRemove.Length == 0)
            return;

        HashSet<string> keySet = new(keysToRemove);
        filteredSteps.RemoveAll(step => keySet.Contains(step));
        foreach (var key in keysToRemove)
        {
            string waifuName = GetWaifuNameFromKey(key);
            if (Enum.TryParse(waifuName, out Waifu waifu))
            {
                waifuUnlockCounts[waifu]++;
            }
        }
    }

    /// Ottiene il numero dello step da una stringa tipo "Chiho_3"
    private bool TryGetStepNumber(string name, out int number)
    {
        number = -1;
        var parts = name.Split('_');
        return parts.Length == 2 && int.TryParse(parts[1], out number);
    }

    private string GetWaifuNameFromKey(string key)
    {
        int index = key.IndexOf('_');
        return index > 0 ? key.Substring(0, index) : key;
    }

    private void UpdateMissingArtTexts()
    {
        if (missingArtText == null || missingArtText.Length == 0)
            return;

        foreach (var textComponent in missingArtText)
        {
            if (textComponent == null || string.IsNullOrEmpty(textComponent.text))
                continue;

            // Estrai waifu name dal testo, partendo dal primo '_' (es. "Chiho_5" => "Chiho")
            string waifuName = ExtractWaifuNameFromText(textComponent.name);
            if (Enum.TryParse<Waifu>(waifuName, ignoreCase: true, out Waifu waifu))
            {
                if (waifuUnlockCounts.TryGetValue(waifu, out int count))
                {
                    // Aggiorna testo con il numero di art sbloccate
                    textComponent.text = count.ToString() + " / 13";
                }
            }
        }
    }

    private string ExtractWaifuNameFromText(string text)
    {
        int index = text.IndexOf('_');
        if (index > 0)
            return text.Substring(0, index);
        else
            return text;
    }

    // Questo metodo accetta un GameObject (es. Pull_1, Pull_10) e carica gli sprite
    public void LoadSpritesFromWaifuList(GameObject parent, List<string> pulledWaifu)
    {
        Image[] images = parent.GetComponentsInChildren<Image>(true);

        int count = Mathf.Min(images.Length, pulledWaifu.Count); // Limita in base al più corto dei due

        for (int i = 0; i < count; i++)
        {
            string fullName = pulledWaifu[i]; // Esempio: "Chiho_1"

            // Estrai la parte prima del primo "_"
            int underscoreIndex = fullName.IndexOf('_');
            if (underscoreIndex < 0)
            {
                Debug.LogWarning($"[WarpPage.cs] Stringa non valida nel formato (manca '_'): {fullName}");
                continue;
            }

            string folder = fullName.Substring(0, underscoreIndex); // es. "Chiho"
            string path = $"Texture/Waifu/{folder}/{fullName}";

            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                images[i].sprite = sprite;
                ExportTextureToDownloads(folder, fullName, out string e); // TODO, modificare in caso voglia mettere un TRADEMARK
            }
            else
            {
                Debug.LogWarning($"[WarpPage.cs] Sprite non trovato nel path: Resources/{path}");
            }
        }
    }

    private IEnumerator FadeInThenOut(CanvasGroup canvasGroup, float fadeDuration = 0.5f, float waitTime = 1.5f)
    {
        if (canvasGroup == null) yield break;

        // Fade In
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        float time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Clamp01(time / fadeDuration);
            if (canvasGroup.alpha != alpha) canvasGroup.alpha = alpha;

            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Wait
        yield return new WaitForSeconds(waitTime);

        // Fade Out
        time = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        while (time < fadeDuration)
        {
            float alpha = 1f - Mathf.Clamp01(time / fadeDuration);
            if (canvasGroup.alpha != alpha) canvasGroup.alpha = alpha;

            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    private void ClearSpritesInGameObject(GameObject parent)
    {
        if (parent == null) return;
        Image[] images = parent.GetComponentsInChildren<Image>(true);
        foreach (var img in images)
        {
            img.sprite = null;
        }
    }
    
    public static bool ExportTextureToDownloads(string waifuName, string fileName, out string errorMessage)
    {
        errorMessage = null;

        // Percorso Resources (senza estensione)
        string resourcePath = $"ExportableTextures/{waifuName}/{fileName}";
        Texture2D texture = Resources.Load<Texture2D>(resourcePath);

        if (texture == null)
        {
            errorMessage = $"Texture non trovata: Resources/{resourcePath}.png";
            return false;
        }

        if (!texture.isReadable)
        {
            errorMessage = $"La texture {fileName} non è leggibile. Abilita 'Read/Write Enabled' da Inspector.";
            return false;
        }

        // Crea una RenderTexture temporanea
        RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);

        // Copia la texture originale sulla RenderTexture (con conversione gamma corretta)
        Graphics.Blit(texture, rt);

        // Attiva la RenderTexture e crea una nuova Texture2D in spazio sRGB
        RenderTexture.active = rt;
        Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false); // false = sRGB

        // Leggi i pixel dalla RenderTexture
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        // Ripristina lo stato
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        // Encode PNG
        byte[] pngBytes = readableTexture.EncodeToPNG();

        if (pngBytes == null || pngBytes.Length == 0)
        {
            errorMessage = "Errore nella codifica in PNG.";
            UnityEngine.Object.Destroy(readableTexture);
            return false;
        }

        // Path cartella Downloads
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        string fullPath = Path.Combine(downloadsPath, fileName + ".png");

        try
        {
            File.WriteAllBytes(fullPath, pngBytes);
            //Debug.Log($"✅ Immagine salvata in: {fullPath}");
        }
        catch (Exception ex)
        {
            errorMessage = $"Errore salvataggio file: {ex.Message}";
            UnityEngine.Object.Destroy(readableTexture);
            return false;
        }

        UnityEngine.Object.Destroy(readableTexture);
        return true;
    }

    private static bool IsCompressedFormat(TextureFormat format)
    {
        return format == TextureFormat.DXT1 || format == TextureFormat.DXT5 || format == TextureFormat.PVRTC_RGB2 ||
            format == TextureFormat.PVRTC_RGBA2 || format == TextureFormat.PVRTC_RGB4 || format == TextureFormat.PVRTC_RGBA4;
    }

    private static Texture2D GetReadableCopy(Texture2D source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTexture = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }
}
