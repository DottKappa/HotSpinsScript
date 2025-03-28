using UnityEngine;
using UnityEngine.UI;

public class PrestigeTable : MonoBehaviour
{
    public GameObject cellOff;      // Riferimento al primo prefab
    public GameObject cellOn;      // Riferimento al secondo prefab
    public Transform gridPanel;     // Riferimento al pannello (GridLayoutGroup)
    public int[] prefabIndices;     // Array di indici che determinano quale prefab usare per ogni cella

    void Start()
    {
        //prefabIndices = new int[10] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // Devo dare il prestigio per il punteggio 
        GenerateTable();
    }

    void GenerateTable()
    {
        int rows = 2;
        int cols = 5;

        // Loop attraverso tutte le celle
        for (int i = 0; i < rows * cols; i++) {
            // Determina quale prefab usare
            GameObject prefabToInstantiate = cellOff;  // Default è cellOff
            if (prefabIndices.Length > i && prefabIndices[i] == 1) {
                prefabToInstantiate = cellOn;  // Se l'indice è 1, usa cellOn
            }

            // Instanzia il prefab direttamente nel gridPanel
            GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, gridPanel);

            // Puoi anche fare in modo che la posizione del prefab si adatti al layout della griglia
            instantiatedPrefab.transform.localPosition = Vector3.zero;

            instantiatedPrefab.transform.localScale = new Vector3(5f, 5f, 5f);
        }
    }
}