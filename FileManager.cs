using System;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string folder = "dataFiles";
    private WaifuFileStructure waifuFile = new WaifuFileStructure(new WaifuSave("Chiho"), new WaifuSave());

    void Awake() 
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folder);

        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        LoadWaifuFile();
    }

    public Waifu GetWaifuName()
    {
        string waifuString = Waifu.Chiho.ToString();
        if (Enum.IsDefined(typeof(Waifu), waifuString)) {
            PlayerPrefs.SetString("waifuName", waifuString);
        } else {
            Debug.LogError("[FileManager.cs] Il nome della waifu non fa parte dell'enum");
        }
        
        // TODO !!
        // Lo prenderà da file 
        // o da una decisione presa prima e salvata in una var globale di unity 
        // o di base avrà Chiho
        return (Waifu)System.Enum.Parse(typeof(Waifu), PlayerPrefs.GetString("waifuName"));
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

            waifuFile = JsonUtility.FromJson<WaifuFileStructure>(json);}
    }
}
