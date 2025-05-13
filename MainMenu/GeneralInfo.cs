using UnityEngine;
using TMPro;

public class GeneralInfo : MonoBehaviour
{
    public TextMeshProUGUI totalPoints;
    public TextMeshProUGUI totalSpins;
    public TextMeshProUGUI totalArtUnlocked;
    private FileManager fileManager;

    void Start()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        SetPoints();
        SetSpins();
        SetArt();
    }

    void SetPoints()
    {
        int total = 0;
        foreach (Waifu waifu in System.Enum.GetValues(typeof(Waifu))) {
            total += fileManager.GetPointsByWaifu(waifu);
        }
        totalPoints.text = addDot(total.ToString());
    }

    void SetSpins()
    {
        int total = 0;
        foreach (Waifu waifu in System.Enum.GetValues(typeof(Waifu))) {
            total += fileManager.GetSpinsByWaifu(waifu);
        }
        totalSpins.text = addDot(total.ToString());
    }

    void SetArt()
    {
        int unlocked = 0;

        // Somma i valori attuali sbloccati per ciascuna waifu
        foreach (Waifu waifu in System.Enum.GetValues(typeof(Waifu))) {
            unlocked += fileManager.GetImageStepByWaifu(waifu);
        }

        // Calcola il numero totale di step disponibili nel gioco
        int total = 0;
        foreach (WaifuSteps step in System.Enum.GetValues(typeof(WaifuSteps))) {
            total++;
        }

        totalArtUnlocked.text = $"{unlocked}/{total}";
    }

    private string addDot(string points)
    {
        int length = points.Length;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int counter = 0;

        // Partiamo dal termine della stringa e aggiungiamo i punti ogni 3 caratteri
        for (int i = length - 1; i >= 0; i--)
        {
            sb.Insert(0, points[i]);
            counter++;

            // Aggiungi un punto ogni 3 caratteri, ma non alla fine
            if (counter == 3 && i != 0)
            {
                sb.Insert(0, '.');
                counter = 0;
            }
        }

        return sb.ToString();
    }
}
