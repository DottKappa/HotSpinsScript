using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class IdleFileStructure
{
    public List<PowerUpData> PowerUp = new();
    public List<Room> Rooms = new();
    public int UnlockableRoom;

    // === POWERUP ===
    public List<PowerUpData> GetPowerUpList() { return PowerUp; }
    public void SetPowerUpList(List<PowerUpData> newPowerUpList) { PowerUp = newPowerUpList; }

    public bool TryGetPowerUpByType(string type, out PowerUpData foundPowerUp)
    {
        foundPowerUp = PowerUp.Find(p => p.Type == type);
        return foundPowerUp != null;
    }

    public void AddPowerUp(PowerUpData newPowerUp) 
    { 
        PowerUp.Add(newPowerUp); 
    }

    // === ROOMS ===
    public List<Room> GetRooms() { return Rooms; }
    public void SetRooms(List<Room> newRooms) { Rooms = newRooms; }

    public bool TryGetRoomByName(string name, out Room foundRoom)
    {
        foundRoom = Rooms.Find(room => room.Name == name);
        return foundRoom != null;
    }

    public void AddRoom(Room newRoom)
    {
        if (Rooms.Exists(room => room.Name == newRoom.Name)) {
            Debug.LogWarning($"[IdleFileStructure] Room '{newRoom.Name}' already exists!");
            return;
        }

        Rooms.Add(newRoom);
    }
    
    // === UNLOCKABLE ROOM ===
    public int GetUnlockableRoom() { return UnlockableRoom; }
    public void SetUnlockableRoom(int value) { UnlockableRoom = value; }
}

[Serializable]
public class PowerUpData
{
    public string Type;
    public int Quantity;

    // Getter e Setter
    public string GetTypeName() => Type;
    public void SetTypeName(string type) => Type = type;

    public int GetQuantity() => Quantity;
    public void SetQuantity(int quantity) => Quantity = quantity;

    public void CreateRandomPowerUp(string rarity)
    {
        this.Type = IdleStatic.GetRandomPowerUp(rarity);
        this.Quantity = 1;
    }

    public void CreatePowerUpByType(string type, int quantity = 1)
    {
        this.Type = type;
        this.Quantity = quantity;
    }
}

[Serializable]
public class Room
{
    [SerializeField] private string name;
    [SerializeField] private int lv;
    [SerializeField] private string selectedWaifu;
    [SerializeField] private float timeNextReward;
    [SerializeField] private float timeMultiplier;
    [SerializeField] private bool locked;

    // Getter e Setter
    public string Name { get => name; set => name = value; }
    public int Lv { get => lv; set => lv = value; }
    public string SelectedWaifu { get => selectedWaifu; set => selectedWaifu = value; }
    public float TimeNextReward { get => timeNextReward; set => timeNextReward = value; }
    public float TimeMultiplier { get => timeMultiplier; set => timeMultiplier = value; }
    public bool Locked { get => locked; set => locked = value; }

    public void CreateRoomByName(string roomName)
    {
        if (IdleStatic.ExistsRoom(roomName)) {
            name = roomName;
            lv = 1;
            selectedWaifu = "";
            timeNextReward = IdleStatic.GetRoomDurationByRoomName(roomName);
            timeMultiplier = 1f;
            locked = false;
        } else {
            Debug.LogError("[IdleFileStructure] La stanza [" + roomName + "] non esiste");
        }
    }
}
