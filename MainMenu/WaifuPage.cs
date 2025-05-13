using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class WaifuPage : MonoBehaviour
{
    private string waifuName;
    private string points;
    private string spins;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI spinsText;
    public GameObject waifuDetailPrefab; // Riferimento al prefab
    public RectTransform contentTransform; // Riferimento al content della ScrollView
    private CollectionWaifu collectionWaifu;
    [SerializeField] private TextMeshProUGUI[] waifuInfoFields;
    private FileManager fileManager;
    private bool isFirstLocked = true;

    public void InitializeWaifu(string waifuName, string points, string spins)
    {
        this.waifuName = waifuName;
        this.points = points;
        this.spins = spins;
        SetText();
        SetPrestigeTable();
    }

    void Start()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        GetCollectionWaifuScript();
        int waifuStep = GetWaifuStep();
        int i = 1;
        bool nextStepExist = true;

        while (nextStepExist) {
            string index = i.ToString();
            if (Enum.TryParse(waifuName+"_"+index, out WaifuSteps result)) {
                GameObject imageToggleInstance = Instantiate(waifuDetailPrefab, contentTransform);
                WaifuDetail imageToggleScript = imageToggleInstance.GetComponentInChildren<WaifuDetail>();
                
                string buttonImagePath = "Texture/Waifu/"+waifuName+"/"+waifuName+"_"+index;
                string fullScreenImagePath = "Texture/Waifu/"+waifuName+"/"+waifuName+"_"+index; // TODO-> deprecato? -> "Texture/Waifu/"+waifuName+"/FullScreen/"+waifuName+"_"+index;
                bool isButtonEnabled = false;
                bool needBlur = false;

                if (i <= waifuStep) {
                    isButtonEnabled = true;
                } else if (isFirstLocked) {
                    needBlur = true;
                    isFirstLocked = false;
                } else {
                    buttonImagePath = "Texture/Waifu/Lock";
                }

                i++;
                imageToggleScript.Initialize(buttonImagePath, fullScreenImagePath, isButtonEnabled, needBlur);
            } else {
                nextStepExist = false;
                break;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);
        SetWaifuInfoData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    private void SetText()
    {
        titleText.text = waifuName;
        pointsText.text = addDot(points);
        spinsText.text = addDot(spins);
    }

    private void SetPrestigeTable()
    {
        int[] prestigeArray = new int[10];
        int pointsInt;
        int.TryParse(points, out pointsInt);
        int[] waifuSteps = GetEnumValuesStartingWith(waifuName);

        for (int i = 0; i < prestigeArray.Length; i++) {
            if (pointsInt >= waifuSteps[i]) {
                prestigeArray[i] = 1;
            } else {
                prestigeArray[i] = 0;
            }
        }

        GameObject table = GameObject.Find("Table");

        if (table != null) {
            PrestigeTable prestigeTableScript = table.GetComponent<PrestigeTable>();

            if (prestigeTableScript != null) {
                prestigeTableScript.prefabIndices = prestigeArray;
                prestigeTableScript.SetUpPrestigeTable(waifuName);
            } else {
                Debug.LogError("PrestigeTable script non trovato sul GameObject Table.");
            }
        } else {
            Debug.LogError("GameObject Table non trovato nella scena.");
        }
    }

    private void GetCollectionWaifuScript()
    {
        // TODO -> Attenzione se cambierò i nomi
        GameObject collectionPage = GameObject.Find("CollectionPage");
        if (collectionPage == null) {
            Debug.LogError("ERRORE!");
        }
        // Trova Chiho anche se disattivato
        Transform chihoTransform = FindChildRecursive(collectionPage.transform, "Chiho");
        if (chihoTransform != null) {
            GameObject chiho = chihoTransform.gameObject;
            collectionWaifu = chiho.GetComponent<CollectionWaifu>();

            if (collectionWaifu == null) {
                Debug.LogError("[WaifuPage.cs] Componente CollectionWaifu non trovato su Chiho!");
            }
        } else {
            Debug.LogError("[WaifuPage.cs] GameObject 'Chiho' non trovato nella scena!");
        }
    }

    private int GetWaifuStep()
    {
        Waifu waifuEnum;
        if (Enum.TryParse(waifuName, out waifuEnum)) {
            return fileManager.GetImageStepByWaifu(waifuEnum);
        }

        return 1;
    }

    public void ReturnButton()
    {
        collectionWaifu.InteractWithGameObj(true);
        Destroy(gameObject);
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

    // Funzione ricorsiva per trovare un figlio anche se disattivato
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        // Controlla se il nome del figlio corrisponde
        if (parent.name == childName) {
            return parent;
        }

        // Ricerca tra tutti i figli
        foreach (Transform child in parent) {
            Transform result = FindChildRecursive(child, childName);
            if (result != null) {
                return result;
            }
        }

        return null; // Non trovato
    }

    private int[] GetEnumValuesStartingWith(string prefix)
    {
        return Enum.GetNames(typeof(WaifuSteps))
                   .Where(name => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                   .Select(name => (int)Enum.Parse(typeof(WaifuSteps), name))
                   .ToArray();
    }

    private void SetWaifuInfoData()
    {
        string[] info = WaifuInfoStatic.GetInfoByWaifu(waifuName.ToLower());
        
        if (info.Length > 0) {
            int waifuStep = GetWaifuStep();
            int index = 2;

            for (int i = 0; i < waifuInfoFields.Length && i < info.Length; i++) {
                if (waifuInfoFields[i] != null && waifuStep >= index) {
                    waifuInfoFields[i].text = info[i];
                    index += 2;
                }
            }
        } else {
            Debug.LogError("[WaifuPage.cs] Non esistono le info per la waifu " + waifuName + " nella classe [WaifuInfoStatic.cs]");
        }
    }
}