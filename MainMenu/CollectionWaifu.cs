using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CollectionWaifu : MonoBehaviour
{
    private string waifuName;
    public Button isActiveButton;
    private string greenHex = "#87FF7E";
    private string redHex = "#FF7E94";
    private CollectionPage collectionPage;
    private GameObject returnButton;
    private FileManager fileManager;
    public GameObject waifuPagePrefab;  // Il prefab di WaifuPage da istanziare

    void Start() {
        collectionPage = FindFirstObjectByType<CollectionPage>();
        fileManager = FindFirstObjectByType<FileManager>();
        returnButton = GameObject.Find("Return");

        waifuName = Capitalize(gameObject.name);
        if (PlayerPrefs.GetString("waifuName") == waifuName) {
            SetIsActiveButton(greenHex, "active");
        }
    }

    public void OpenWaifuPage()
    {
        GameObject waifuInstance = Instantiate(waifuPagePrefab);
        WaifuPage waifuPage = waifuInstance.GetComponent<WaifuPage>();
        if (waifuPage != null) {
            waifuPage.InitializeWaifu(waifuName, GetWaifuPoints(), GetWaifuSpins());
            InteractWithGameObj(false);
        } else {
            Debug.LogError("[CollectionWaifu.cs] WaifuPage non trovato nel prefab");
        }
    }

    public void SetWaifuActive()
    {
        if (WaifuExistInEnum()) {
            PlayerPrefs.SetString("waifuName", waifuName);
            collectionPage.SetAllChildInactive();
            SetIsActiveButton(greenHex, "active");
            Debug.Log("ho settato correttamente");
        }
    }

    public void SetWaifuInactive()
    {
        SetIsActiveButton(redHex, "inactive");
    }

    private bool WaifuExistInEnum()
    {
        if (Enum.IsDefined(typeof(Waifu), waifuName)) {
            return true;
        }

        Debug.LogError("[CollectionWaifu.cs] Il nome della waifu [" + waifuName + "] non fa parte dell'enum");
        return false;
    }

    private void SetIsActiveButton(string hexColor, string newText)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor)) {
            // Imposta la trasparenza (Alpha) a 0.2 (20% di trasparenza)
            newColor.a = 0.2f;

            // Cambia il colore del bottone con la trasparenza
            isActiveButton.GetComponent<Image>().color = newColor;
        }

        isActiveButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
    }

    private string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // La prima lettera in maiuscolo e il resto in minuscolo
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    public void InteractWithGameObj(bool setIsActive)
    {
        collectionPage.gameObject.SetActive(setIsActive);
        returnButton.SetActive(setIsActive);
    }

    private string GetWaifuPoints()
    {
        Waifu waifuEnum;
        if (Enum.TryParse(waifuName, out waifuEnum)) {
            return fileManager.GetPointsByWaifu(waifuEnum).ToString();
        }

        return "0";
    }

    private string GetWaifuSpins()
    {
        Waifu waifuEnum;
        if (Enum.TryParse(waifuName, out waifuEnum)) {
            return fileManager.GetSpinsByWaifu(waifuEnum).ToString();
        }

        return "0";
    }
}
