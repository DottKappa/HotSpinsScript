using UnityEngine;
using System;
using System.Reflection;

// Classe che va sincronizzata con Steamworks 56 achievements 1.0
public static class SteamBackEndStatic
{
    // TODO -> Per quello di avere tutti gli achievement come faccio? Dovrei chiamare per sapere se li ho sbloccati tutti

    private static readonly string[] generalAchievement_1 = new string[]
    {
        "FIRST_SPIN",
        "POINTS_1",
        "POINTS_2",
        "POINTS_3",
        "FINAL_ACHIEVEMENT_1",
    };

    private static readonly string[] idleAchievement_1 = new string[]
    {
        "IDLE_UNLOCK_1",
        "IDLE_UNLOCK_2",
        "IDLE_UNLOCK_3",
        "IDLE_UNLOCK_4",
        "IDLE_UNLOCK_5",
        "IDLE_MAX_TIME_LV",
        "IDLE_MAX_ROOM_LV",
    };

    private static readonly string[] chihoAchievement_1 = new string[]
    {
        "CHIHO_SPIN_1",
        "CHIHO_SPIN_2",
        "CHIHO_SPIN_3",
        "CHIHO_UNLOCK_2",
        "CHIHO_UNLOCK_3",
        "CHIHO_UNLOCK_4",
        "CHIHO_WATCHME_1",
        "CHIHO_WATCHME_2",
    };

    private static readonly string[] hinaAchievement_1 = new string[]
    {
        "HINA_SPIN_1",
        "HINA_SPIN_2",
        "HINA_SPIN_3",
        "HINA_UNLOCK_1",
        "HINA_UNLOCK_2",
        "HINA_UNLOCK_3",
        "HINA_UNLOCK_4",
        "HINA_WATCHME_1",
        "HINA_WATCHME_2",
    };

    private static readonly string[] shioriAchievement_1 = new string[]
    {
        "SHIORI_SPIN_1",
        "SHIORI_SPIN_2",
        "SHIORI_SPIN_3",
        "SHIORI_UNLOCK_1",
        "SHIORI_UNLOCK_2",
        "SHIORI_UNLOCK_3",
        "SHIORI_UNLOCK_4",
        "SHIORI_WATCHME_1",
        "SHIORI_WATCHME_2",
    };

    private static readonly string[] tsukikoAchievement_1 = new string[]
    {
        "TSUKIKO_SPIN_1",
        "TSUKIKO_SPIN_2",
        "TSUKIKO_SPIN_3",
        "TSUKIKO_UNLOCK_1",
        "TSUKIKO_UNLOCK_2",
        "TSUKIKO_UNLOCK_3",
        "TSUKIKO_UNLOCK_4",
        "TSUKIKO_WATCHME_1",
        "TSUKIKO_WATCHME_2",
    };

    private static readonly string[] soojinAchievement_1 = new string[]
    {
        "SOOJIN_SPIN_1",
        "SOOJIN_SPIN_2",
        "SOOJIN_SPIN_3",
        "SOOJIN_UNLOCK_1",
        "SOOJIN_UNLOCK_2",
        "SOOJIN_UNLOCK_3",
        "SOOJIN_UNLOCK_4",
        "SOOJIN_WATCHME_1",
        "SOOJIN_WATCHME_2",
    };

    public static string[] GetAchievementByWaifuNameAndSet(string waifuName, string achievementSet)
    {
        string fieldName = waifuName.ToLower() + "Achievement_" + achievementSet;
        FieldInfo field = typeof(SteamBackEndStatic).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

        if (field != null && field.FieldType == typeof(string[]))
        {
            return (string[])field.GetValue(null);
        }

        return Array.Empty<string>();
    }

    public static string[] GetAchievementGeneral()
    {
        return generalAchievement_1;
    }

    public static string[] GetAchievementIdle()
    {
        return idleAchievement_1;
    }
}
