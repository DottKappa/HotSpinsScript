using System;
using System.IO;
using UnityEngine;

public class IdleFileManager : MonoBehaviour
{
    private string folder = "dataFiles";
    private IdleFileStructure idleFileStructure = new IdleFileStructure();
    private IdleManager idleManager;

    private void Awake() 
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folder);

        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }

        LoadIdleFile();
    }

    void Start()
    {
        idleManager = FindFirstObjectByType<IdleManager>();
    }
//    Debug.Log(idleFileStructure.TryGetRoomByName("hall").Lv);

    public PowerUpData GetPowerUpByName(string powerUpName)
    {
        return idleFileStructure.TryGetPowerUpByType(powerUpName, out PowerUpData powerUp) ? powerUp : null;
    }

    public Room GetRoomByName(string roomName)
    {
        return idleFileStructure.TryGetRoomByName(roomName, out var room) ? room : null;
    }

    public Room GetOrCreateRoomByName(string roomName)
    {
        Room room = GetRoomByName(roomName);
        if (room == null) {
            room = new Room();
            room.CreateRoomByName(roomName);
            idleFileStructure.AddRoom(room);
            Debug.Log("[IdleFileManager] Ho creato la nuova stanza [" + roomName + "] nel idleFileStructure");
        }

        return room;
    }

    public string GetActiveWaifuByRoomName(string roomName)
    {
        Room room = GetRoomByName(roomName);
        if (room == null) {return null;}
        return room.SelectedWaifu;
    }

    public int GetNumberOfUnlockableRoom()
    {
        return idleFileStructure.UnlockableRoom;
    }

    public int UseUnlockableRoom(int use = 1)
    {
        int numberOfUnlockable = GetNumberOfUnlockableRoom();
        if (numberOfUnlockable < use) {
            throw new InvalidOperationException("[IdleFileManager.cs] Impossibile usare un unclockableRoom, nel file ne sono presenti [" + numberOfUnlockable + "]");
        }

        idleFileStructure.UnlockableRoom = numberOfUnlockable - use;
        return idleFileStructure.UnlockableRoom;
    }

    public int GetRoomLvByName(string roomName)
    {
        Room room = GetRoomByName(roomName);
        return room.Lv;
    }

    public void SetRoomLvByName(string roomName, int lv)
    {
        Room room = GetRoomByName(roomName);
        room.Lv = lv;
    }

    public void SaveIdleFile()
    {
        string nameFile = "idleData.json";
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);
        SaveAllRooms();

        try {
            string json = JsonUtility.ToJson(idleFileStructure);
            File.WriteAllText(filePath, json);
            Debug.Log("[" + nameFile + "] Salvato correttamente");
        } catch (System.Exception e) {
            Debug.LogError("[IdleFileManager] Failed to save waifu file: " + e.Message);
        }
    }

    private void LoadIdleFile()
    {
        string nameFile = "idleData.json";
        string filePath = Path.Combine(Path.Combine(Application.persistentDataPath, folder), nameFile);

        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            if (json == "{}" || string.IsNullOrEmpty(json)) {
                Debug.LogWarning("[IdleFileManager] Il file json è vuoto");
                return;
            }

            idleFileStructure = JsonUtility.FromJson<IdleFileStructure>(json);
        } else {
            try {
                CreateBasicFile(nameFile, filePath);
            } catch (System.Exception e) {
                Debug.LogError("[IdleFileManager] Failed to save waifu file: " + e.Message);
            }
        }
    }

    private void CreateBasicFile(string nameFile, string filePath)
    {
        Room newBasicRoom = new Room();
        newBasicRoom.CreateRoomByName(IdleStatic.GetBasicRoom());
        idleFileStructure.AddRoom(newBasicRoom);
        string json = JsonUtility.ToJson(idleFileStructure);
        File.WriteAllText(filePath, json);
        Debug.Log("[" + nameFile + "] Salvato correttamente");
    }

    private void SaveAllRooms()
    {
        idleManager = FindFirstObjectByType<IdleManager>();
        string[] allRooms = IdleStatic.GetAllRooms();
        for (int i = 0; i < allRooms.Length; i++) {
            GameObject room = GameObject.Find(allRooms[i]);
            if (room != null) {
                // Se non c'è, ma c'è nella scena vuol dire che l'ho appena sbloccata
                Room fileRoom = GetOrCreateRoomByName(allRooms[i]);

// PROGRESS BAR
                Transform progressBar = room.transform.Find("ProgressObj");
                if (progressBar != null) {
                    var progressBarScript = progressBar.GetComponent<ProgressBarTimer>();
                    if (progressBarScript != null) {
                        fileRoom.TimeNextReward = progressBarScript.GetTimeForNextReward();
                    } else {
                        Debug.LogError("[IdleFileManager][SAVE] Impossibile trovare lo script della barra del progresso [" + allRooms[i] + "]");
                        continue;
                    }
                } else {
                    Debug.LogError("[IdleFileManager][SAVE] Impossibile trovare la barra del prograsso [" + allRooms[i] + "]");
                    continue;
                }
// WAIFU ACTIVE
                string waifuActive = null;
                try {
                    waifuActive = idleManager.GetWaifuActiveByRoomName(allRooms[i]);
                } catch (ArgumentNullException ex) {
                    Debug.LogError("Eccezione nella ricerca della waifu attiva per la stanza [" + allRooms[i] + "]: " + ex.Message);
                    continue;
                } catch (InvalidOperationException ex) {
                    Debug.LogError("Eccezione nella ricerca della waifu attiva per la stanza [" + allRooms[i] + "]: " + ex.Message);
                    continue;
                }
                if (waifuActive != null) {
                    fileRoom.SelectedWaifu = waifuActive;
                } else {
                    Debug.LogError("[IdleFileManager][SAVE] Impossibile trovare la waifu attiva [" + allRooms[i] + "]");
                    continue;
                }

//


            } else {
                Debug.LogWarning("[IdleFileManager][SAVE] Impossibile trovare la stanza [" + allRooms[i] + "]");
            }
        }
    }
}
