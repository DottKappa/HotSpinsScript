using UnityEngine;
using System;
using System.Collections.Generic;
using Steamworks; // TODO da cancellare

public class SteamAchievement : MonoBehaviour
{
    public static SteamAchievement Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // mantiene l'oggetto anche nei cambi scena
        }
        else
        {
            Destroy(gameObject); // distrugge duplicati
        }
    }

    // Da cancellare Start()
    void Start()
    {
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.StoreStats();
    }

    public void CheckAchievementByWaifuFile(WaifuSave[] modifiedWaifus)
    {
        foreach (var waifuSave in modifiedWaifus)
        {
            string waifuName = waifuSave.GetWaifuName();
            string[] achievements = SteamBackEndStatic.GetAchievementByWaifuNameAndSet(waifuName, "1");

            int spins = waifuSave.GetSpins();
            int imageStep = waifuSave.GetImageStep();
            int secondsFullScreen = waifuSave.GetSecondsInFullScreen();
            string waifuNameLower = waifuName.ToLower();

            var spinAchievements = new Dictionary<string, int>
            {
                { "_SPIN_1", 250 },
                { "_SPIN_2", 500 },
                { "_SPIN_3", 999 }
            };

            var unlockAchievements = new Dictionary<string, int>
            {
                { "_UNLOCK_1", 1 },
                { "_UNLOCK_2", 4 },
                { "_UNLOCK_3", 7 },
                { "_UNLOCK_4", 10 }
            };

            var timeAchievements = new Dictionary<string, int>
            {
                { "_WATCHME_1", 600 },
                { "_WATCHME_2", 1200 },
            };

            foreach (var achievementName in achievements)
            {
                // Check spin achievements
                foreach (var pair in spinAchievements)
                {
                    if (achievementName.Contains(pair.Key) && spins >= pair.Value)
                    {
                        SteamAchievementManager.Instance.AwardAchievement(achievementName);
                        break;
                    }
                }

                // Check unlock achievements
                foreach (var pair in unlockAchievements)
                {
                    if (achievementName.Contains(pair.Key))
                    {
                        if (pair.Key == "_UNLOCK_1" && waifuNameLower == "chiho")
                            continue;

                        if (imageStep >= pair.Value)
                        {
                            SteamAchievementManager.Instance.AwardAchievement(achievementName);
                        }

                        break;
                    }
                }

                // Check time full screen achievements
                foreach (var pair in timeAchievements)
                {
                    if (achievementName.Contains(pair.Key) && secondsFullScreen >= pair.Value)
                    {
                        SteamAchievementManager.Instance.AwardAchievement(achievementName);
                    }
                }
            }
        }
    }

    public void CheckGenericAchievement(WaifuFileStructure waifuFile)
    {
        var spinAchievements = new Dictionary<string, int>
        {
            { "POINTS_1", 250000 },
            { "POINTS_2", 500000 },
            { "POINTS_3", 999999 }
        };

        string[] achievements = SteamBackEndStatic.GetAchievementGeneral();
        int total = 0;
        foreach (Waifu waifu in System.Enum.GetValues(typeof(Waifu)))
        {
            total += waifuFile.GetWaifuDataByName(waifu).GetPoints();
        }

        foreach (var achievementName in achievements)
        {
            foreach (var pair in spinAchievements)
            {
                if (achievementName.Contains(pair.Key) && total >= pair.Value)
                {
                    SteamAchievementManager.Instance.AwardAchievement(achievementName);
                }
            }

            if (achievementName == "FINAL_ACHIEVEMENT_1" && SteamAchievementManager.Instance.GetNumberOfMissingAchievement1() == 1)
            {
                SteamAchievementManager.Instance.AwardAchievement(achievementName);
            }
        }
    }
}
