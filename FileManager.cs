using System;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string folder = "dataFiles";
    private WaifuFileStructure waifuFile = new WaifuFileStructure(new WaifuSave("Chiho"), new WaifuSave());

    void Start() {

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
        return waifuSave.GetPoints();
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

    public void SaveWaifuFile()
    {
        string nameFile = "waifuData.json";

    }

    public void LoadWaifuFile()
    {
        string nameFile = "waifuData.json";
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);

        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            waifuFile = JsonUtility.FromJson<WaifuFileStructure>(json);
        }
    }
}
