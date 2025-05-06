using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class WaifuLockedScreen : MonoBehaviour
{
    private FileManager fileManager;
    private bool isUnlocked = false;

    void Start()
    {
        GameObject fileManagerObject = GameObject.Find("FileManager");
        if (fileManagerObject != null) {
            fileManager = fileManagerObject.GetComponent<FileManager>();
        } else {
            throw new InvalidOperationException("[WaifuLockedScreen.cs] Non ho trovato l'elemento FileManager");
        }

        string parentName = transform.parent.name;
        int pointsNeeded = 0;

        isUnlocked = GetUnlocked(parentName);
        if (!isUnlocked) {
            pointsNeeded = FindWaifuStep6(parentName);
            if (pointsNeeded <= 0) {
                isUnlocked = true;
                SetUnlocked(parentName);
            } else {
                if (FindPointsOldWaifu(parentName) >= pointsNeeded) {
                    isUnlocked = true;
                    SetUnlocked(parentName);
                }
            }
        }

        if (isUnlocked) {
            Destroy(gameObject);
        } else {
            SetUnlockText(addDot(pointsNeeded), GetOldWaifu(parentName));
        }
    }

    private bool GetUnlocked(string waifuName)
    {
        return fileManager.GetIsUnlockedByWaifu((Waifu)System.Enum.Parse(typeof(Waifu), waifuName));
    }

    private void SetUnlocked(string waifuName)
    {
        fileManager.SetIsUnlockedByWaifu(true, (Waifu)System.Enum.Parse(typeof(Waifu), waifuName));
    }

    private int FindWaifuStep6(string waifuName)
    {
        var waifus = Enum.GetNames(typeof(Waifu));
        int index = Array.IndexOf(waifus, waifuName);
        if (index <= 0) return 0;

        string waifuPrecedente = waifus[index - 1];
        var enumValue = (WaifuSteps)Enum.Parse(typeof(WaifuSteps), waifuPrecedente + "_6");
        return (int)enumValue;
    }

    private int FindPointsOldWaifu(string waifuName)
    {
        var waifus = Enum.GetNames(typeof(Waifu));
        int index = Array.IndexOf(waifus, waifuName);
        string waifuPrecedente = waifus[index - 1];
        return fileManager.GetPointsByWaifu((Waifu)System.Enum.Parse(typeof(Waifu), waifuPrecedente));
    }

    private string GetOldWaifu(string waifuName)
    {
        var waifus = Enum.GetNames(typeof(Waifu));
        int index = Array.IndexOf(waifus, waifuName);
        return waifus[index - 1];
    }

    private void SetUnlockText(string pointsNeeded, string waifuNeeded)
    {
        var tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) {
            tmp.text = "Unlock at " + pointsNeeded + " points on " + waifuNeeded;
        }
    }

    private string addDot(int points)
    {
        string pointsString = points.ToString();
        int length = pointsString.Length;

        if (length <= 3) return pointsString;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int counter = 0;

        for (int i = length - 1; i >= 0; i--) {
            sb.Insert(0, pointsString[i]);
            counter++;
            if (counter == 3 && i != 0) {
                sb.Insert(0, '.');
                counter = 0;
            }
        }

        return sb.ToString();
    }
}
