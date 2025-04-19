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

    // Se si aggiunge una stanza va aggiunto la relativa durata di base
    private static readonly float[] roomsDuration = new float[]
    {
        3600f,
        3300f,
        3000f,
        2700f,
        2400f,
        2100f
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

// == POWERUPS
    private static readonly string[] rarities = new string[]
    {
        "common",
        "rare",
        "mythic"
    };

    private static readonly string[] idleCommonPowerUps = new string[]
    {
        "3x_on_horizontal"
    };

    private static readonly string[] idleRarePowerUps = new string[]
    {
        "5x_on_horizontal"
    };

    private static readonly string[] idleMythicPowerUps = new string[]
    {
        "10x_on_horizontal"
    };

    public static string[] GetRarities()
    {
        return rarities;
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
