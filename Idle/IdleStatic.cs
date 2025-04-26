using System;
using System.Linq;

public static class IdleStatic
{
// == ROOMS
    private static readonly string[] rooms = new string[]
    {
        "Hall",
        "Slot",
        "Poker",
        "Bar",
        "Reception",
        "Kitchen"
    };

    // Se si aggiunge una stanza va aggiunto la relativa durata di base..
    private static readonly float[] roomsDuration = new float[]
    {
        3600f,
        3300f,
        3000f,
        2700f,
        2400f,
        2100f
    };

    //..E il costo in sbloccabili
    private static readonly float[] roomsUnlockableNeeded = new float[]
    {
        0f,
        1f,
        2f,
        3f,
        4f,
        4.5f, // il .5 significa che necessita tutti quelli prima sbloccati + 4 unlockable
    };

    public static string GetBasicRoom()
    {
        return rooms[0];
    }

    public static string[] GetAllRooms()
    {
        return rooms;
    }

    public static bool ExistsRoom(string roomName)
    {
        return rooms.Contains(roomName);
    }

    public static float GetRoomDurationByRoomName(string roomName)
    {
        for (int i = 0; i < rooms.Length; i++) {
            if (rooms[i] == roomName) {
                return roomsDuration[i];
            }
        }

        return roomsDuration[0] * 2;
    }

    public static float GetUnlockableNeededByRoomName(string roomName)
    {
        if (ExistsRoom(roomName)) {
            for (int i = 0; i < rooms.Length; i++) {
                if (roomName == rooms[i]) {
                    return roomsUnlockableNeeded[i];
                }
            }    
        }

        throw new Exception("[IdleStatic.cs] La stanza passata [" + roomName + "] non esiste o non ha un corrispettivo di sbloccabili");
    }

// == POWERUPS
    private static readonly string[] rarities = new string[]
    {
        "common",
        "rare",
        "mythic"
    };

    private static readonly float[] weightsLv1 = new float[]
    {
        90f,
        10f,
        0f
    };

    private static readonly float[] weightsLv2 = new float[]
    {
        65f,
        27f,
        8f
    };

    private static readonly float[] weightsLv3 = new float[]
    {
        30f,
        54f,
        16f
    };


    private static readonly string[] idleCommonPowerUps = new string[]
    {
        "3x_on_horizontal"
    };

    private static readonly string[] idleCommonPowerUpsDesc = new string[]
    {
        "Multiply horizontal line 3 times"
    };

    private static readonly string[] idleRarePowerUps = new string[]
    {
        "5x_on_horizontal"
    };

    private static readonly string[] idleRarePowerUpsDesc = new string[]
    {
        "Multiply horizontal line 5 times"
    };

    private static readonly string[] idleMythicPowerUps = new string[]
    {
        "10x_on_horizontal"
    };

    private static readonly string[] idleMythicPowerUpsDesc = new string[]
    {
        "Multiply horizontal line 10 times"
    };

    public static string[] GetRarities()
    {
        return rarities;
    }

    public static string GetRarityByPowerUpName(string title)
    {
        if (Array.Exists(idleCommonPowerUps, p => p == title)) return "common";
        if (Array.Exists(idleRarePowerUps, p => p == title)) return "rare";
        if (Array.Exists(idleMythicPowerUps, p => p == title)) return "mythic";
        return null;
    }

    public static string GetPowerUpDescriptionByTitle(string title)
    {
        // Common
        for (int i = 0; i < idleCommonPowerUps.Length; i++){
            if (idleCommonPowerUps[i] == title)
                return idleCommonPowerUpsDesc[i];
        }

        // Rare
        for (int i = 0; i < idleRarePowerUps.Length; i++) {
            if (idleRarePowerUps[i] == title)
                return idleRarePowerUpsDesc[i];
        }

        // Mythic
        for (int i = 0; i < idleMythicPowerUps.Length; i++) {
            if (idleMythicPowerUps[i] == title)
                return idleMythicPowerUpsDesc[i];
        }

        return null;
    }

    public static string GetRandomPowerUp(string rarity)
    {
        if (string.IsNullOrWhiteSpace(rarity)) {
            var rand = new Random();
            string [] rarities = IdleStatic.GetRarities();
            rarity = rarities[rand.Next(rarities.Length)];
        }

        return GetRandomPowerUpByRarity(rarity);
    }

    public static float[] GetWeightsByRoomLv(int lv)
    {
        switch (lv) {
            case 1: return weightsLv1;
            case 2: return weightsLv2;
            case 3: return weightsLv3;
        }

        return weightsLv1;
    }

    private static string GetRandomPowerUpByRarity(string rarity)
    {
        var rand = new Random();
        string[] powerUps;

        switch (rarity.ToLower()) {
            case "common":
                powerUps = idleCommonPowerUps;
                break;
            case "rare":
                powerUps = idleRarePowerUps;
                break;
            case "mythic":
                powerUps = idleMythicPowerUps;
                break;
            default:
                return null;
        }

        if (powerUps.Length == 0) {
            //Debug.LogError("[IdleStatic] Non è stato possibile recuperare un powerUp random dalla rarità [" + rarity + "]");
            return null;
        }

        return powerUps[rand.Next(powerUps.Length)];
    }
}
