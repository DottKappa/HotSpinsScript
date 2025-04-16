using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Globalization;

public class PrestigeTable : MonoBehaviour
{
    public GameObject cellOff;      // Riferimento al primo prefab
    public GameObject cellOn;      // Riferimento al secondo prefab
    public Transform gridPanel;     // Riferimento al pannello (GridLayoutGroup)
    public int[] prefabIndices;     // Array di indici che determinano quale prefab usare per ogni cella
    private int[] waifuSteps;

    public void SetUpPrestigeTable(string waifuName)
    {
        waifuSteps = GetEnumValuesStartingWith(waifuName);
        GenerateTable();
    }

    void GenerateTable()
    {
        int rows = 2;
        int cols = 5;

        int waifuIndex = 0;
        bool firstHidden = true;

        for (int i = 0; i < rows * cols; i++)
        {
            GameObject prefabToInstantiate = cellOff;
            bool isCellOn = false;

            if (prefabIndices.Length > i && prefabIndices[i] == 1) {
                prefabToInstantiate = cellOn;
                isCellOn = true;
            }

            GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, gridPanel);
            instantiatedPrefab.transform.localPosition = Vector3.zero;
            instantiatedPrefab.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);

            if (isCellOn || firstHidden) {
                Transform hoverTextTransform = instantiatedPrefab.transform.Find("HoverText");
                if (hoverTextTransform != null) {
                    TMP_Text tmpText = hoverTextTransform.GetComponent<TMP_Text>();
                    if (tmpText != null && waifuIndex < waifuSteps.Length) {
                        tmpText.text = waifuSteps[waifuIndex].ToString("N0", new CultureInfo("it-IT")); // formato con separatori (es: 35,000)
                        waifuIndex++;
                    }
                }
                if (!isCellOn) {
                    firstHidden = false;
                }
            }
        }
    }

    private int[] GetEnumValuesStartingWith(string prefix)
    {
        return Enum.GetNames(typeof(WaifuSteps))
                   .Where(name => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                   .Select(name => (int)Enum.Parse(typeof(WaifuSteps), name))
                   .ToArray();
    }
}