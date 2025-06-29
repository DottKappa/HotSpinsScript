using UnityEngine;
using Steamworks;
using System;
using System.Linq;
using System.Reflection;

public class SteamAchievementManager : MonoBehaviour
{
    public static SteamAchievementManager Instance { get; private set; }
    private int missingAchievements_1;

    private void Awake()
    {
        // Singleton base
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Verifica che Steam sia attivo
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam non inizializzato!");
        }
    }

    private void Start()
    {
        missingAchievements_1 = GetMissingAchievementsCount("Achievement_1");
    }

    public void AwardAchievement(string achievementId)
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam non è inizializzato, non posso assegnare achievement.");
            return;
        }

        if (SteamUserStats.SetAchievement(achievementId))
        {
            //Debug.Log($"Achievement {achievementId} sbloccato!");
            SteamUserStats.StoreStats();
            missingAchievements_1 = GetMissingAchievementsCount("Achievement_1");
        }
    }

    public static int GetMissingAchievementsCount(string suffix)
    {
        int missingCount = 0;

        // Cerca i campi statici il cui nome termina con il suffisso specificato
        FieldInfo[] achievementFields = typeof(SteamBackEndStatic)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(f => f.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) && f.FieldType == typeof(string[]))
            .ToArray();

        foreach (var field in achievementFields)
        {
            string[] achievements = (string[])field.GetValue(null);

            foreach (string achievementId in achievements)
            {
                bool achieved;
                if (SteamUserStats.GetAchievement(achievementId, out achieved))
                {
                    if (!achieved)
                        missingCount++;
                }
            }
        }

        return missingCount;
    }

    public int GetNumberOfMissingAchievement1()
    {
        return missingAchievements_1;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoCreateManager()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("SteamAchievementManager");
            obj.AddComponent<SteamAchievementManager>();
        }
    }
}
