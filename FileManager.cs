using System;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string folder = "dataFiles";
    private WaifuFileStructure waifuFile = new WaifuFileStructure(new WaifuSave("Chiho"), new WaifuSave("Hina"), new WaifuSave("Shiori"));
    private SelectorSkin selectorSkin;

    void Awake() 
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folder);

        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        LoadWaifuFile();
    }

    void Start()
    {
        selectorSkin = FindFirstObjectByType<SelectorSkin>();
    }

    public Waifu GetActiveWaifuName()
    {
        return (Waifu)System.Enum.Parse(typeof(Waifu), PlayerPrefs.GetString("waifuName"));
    }

    public bool GetIsUnlockedByWaifu(Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetIsUnlocked();
    }

    public int GetPointsByWaifu(Waifu waifuName = Waifu.Chiho)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetPoints();
    }

    public int GetSpinsByWaifu(Waifu waifuName = Waifu.Chiho)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetSpins();
    }

    public int GetImageStepByWaifu(Waifu waifuName = Waifu.Chiho)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetImageStep();
    }

    public PowerUpUsed<BuffType> GetBuffUsedByWaifu(Waifu waifuName = Waifu.Chiho)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetBuffUsed();
    }
    
    public PowerUpUsed<DebuffType> GetDebuffUsedByWaifu(Waifu waifuName = Waifu.Chiho)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetDebuffUsed();
    }

    public Multiplier GetMultiplierByWaifu(Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetMultiplier();
    }

    public float[] GetWeightsByWaifu(Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        return waifuSave.GetWeights();
    }

    public void SetIsUnlockedByWaifu(bool isUnlocked, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetIsUnlocked(isUnlocked);
    }  

    public void SetPointsByWaifu(int points, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetPoints(points);
    }

    public void SetSpinsByWaifu(int spins, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetSpins(spins);
    }

    public void SetImageStepByWaifu(int imageStep, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetImageStep(imageStep);
    }

    public void SetBuffUsedByWaifu(string[] names, bool[] isUsed, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetBuffUsed(names, isUsed);
    }

    public void SetDebuffUsedByWaifu(string[] names, bool[] isUsed, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetDebuffUsed(names, isUsed);
    }

    public void SetMultiplierByWaifu(MultiplierData horizontal, MultiplierData upDown, MultiplierData downUp, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetMultiplier(horizontal, upDown, downUp);
    }

    public void SetWeightsByWaifu(float[] weights, Waifu waifuName)
    {
        WaifuSave waifuSave = waifuFile.GetWaifuDataByName(waifuName);
        waifuSave.SetWeights(weights);
    }

    public void SaveWaifuFile()
    {
        string nameFile = "waifuData.json";
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);
        
        try {
            string json = JsonUtility.ToJson(waifuFile);
            //Debug.Log("JSON da salvare: " + json);
            File.WriteAllText(filePath, json);
            Debug.Log("[" + nameFile + "] Salvato correttamente");
        } catch (System.Exception e) {
            Debug.LogError("[FileManager] Failed to save waifu file: " + e.Message);
        }

        selectorSkin.SetUpImageButtons();
    }

    public void LoadWaifuFile()
    {
        string nameFile = "waifuData.json";
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);

        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            if (json == "{}" || string.IsNullOrEmpty(json)) {
                Debug.LogWarning("[FileManager] Il file json è vuoto");
                return;
            }

            WaifuFileStructure loadedWaifuFile = JsonUtility.FromJson<WaifuFileStructure>(json);
            foreach (Waifu waifuType in Enum.GetValues(typeof(Waifu))) {
                ReplaceWaifuIfNameMatch(waifuType, loadedWaifuFile);
            }
        }
    }

    private void ReplaceWaifuIfNameMatch(Waifu waifuType, WaifuFileStructure loadedFile)
    {
        WaifuSave current = waifuFile.GetWaifuDataByName(waifuType);
        WaifuSave loaded = loadedFile.GetWaifuDataByName(waifuType);

        if (current == null || loaded == null) {
            //Debug.LogWarning($"[Skip] Dati mancanti per {waifuType}");
            return;
        }

        if (current.GetWaifuName() != loaded.GetWaifuName()) {
            //Debug.Log($"[Skip] waifuName mismatch: current = {current.GetWaifuName()}, loaded = {loaded.GetWaifuName()}");
            return;
        }

        if (current.Equals(loaded)) {
            //Debug.Log($"[Skip] I dati per {waifuType} sono già aggiornati.");
            return;
        }

        // Crea un nuovo WaifuFileStructure con i dati aggiornati
        waifuFile = CreateUpdatedWaifuFile(waifuType, loaded);
    }

    private WaifuFileStructure CreateUpdatedWaifuFile(Waifu toReplace, WaifuSave newData)
    {
        // Recupera i waifu attuali
        WaifuSave chiho = (toReplace == Waifu.Chiho) ? newData : waifuFile.GetWaifuDataByName(Waifu.Chiho);
        WaifuSave hina  = (toReplace == Waifu.Hina)  ? newData : waifuFile.GetWaifuDataByName(Waifu.Hina);
        WaifuSave shiori  = (toReplace == Waifu.Shiori)  ? newData : waifuFile.GetWaifuDataByName(Waifu.Shiori);
        // Qui aggiungo la nuova waifu in futuro
        // WaifuSave misaki = (toReplace == Waifu.Misaki) ? newData : waifuFile.GetWaifuDataByName(Waifu.Misaki);

        return new WaifuFileStructure(chiho, hina, shiori); // Qui dovrò aggiungere misaki +
    }
}
