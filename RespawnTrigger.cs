using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System;
using System.Collections.Generic;

public class RespawnTrigger : MonoBehaviour
{
    private SceneManager sceneManager;
    private FileManager fileManager;
    private float[] weights = new float[13] {10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 6f, 6f, 3.8f, 3.7f, 0.5f};
    private GameObject[] prefabs;
    private float speed = 23.0f;
    private int numberOfSpecialSpins = 0;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        fileManager = FindFirstObjectByType<FileManager>();

        Waifu waifuName = (Waifu)System.Enum.Parse(typeof(Waifu), PlayerPrefs.GetString("waifuName"));
        weights = fileManager.GetWeightsByWaifu(waifuName);
        if (weights == null || weights.Length == 0) {
            weights = GetDefaultWeights();
        }
        prefabs = sceneManager.GetAllPrefabs();
    }

    void Update()
    {
        if (sceneManager.GetNumberOfSpins() > numberOfSpecialSpins) {
            // Resetto la velocità iniziale
            speed = 23.0f;
        }
    }

    public void RespawnAtX(float xPosition)
    {
        if (sceneManager == null) {
            Debug.LogError("SceneManager non è stato assegnato.");
            return;
        }

        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        GameObject prefabToSpawn = null;
        prefabToSpawn = GetRandomGameObjectByWeight();

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        SlotController slotController = spawnedObject.GetComponent<SlotController>();
        if (slotController != null) {
            sceneManager.AddValueToMatrix(spawnedObject, GetColumnForMatrix((float)xPosition));
            slotController.SetConstructorValues(false, speed);
            sceneManager.SetBusy(false);
        }
    }

    int GetColumnForMatrix(float xPosition)
    {
        Vector3[] startingPositions = sceneManager.GetAllStartingPosition();
        for (int i = 0; i < 3; i++) {
            if (Mathf.Approximately(startingPositions[i].x, xPosition)) {
                return i;
            }
        }

        Debug.LogError("Posizione non valida. Controllare l'assegnazione della colonna. Possibile che siano state modificate le x");
        return 0;
    }

    private GameObject GetRandomGameObjectByWeight()
    {
        if (prefabs.Length != weights.Length) {
            Debug.LogError("Il numero di pesi non corrisponde al numero di prefabs. Controllare che i pesi siano aggiornati in RESPAWN_TRIGGER.");
            return null;
        }

        // Calcola la somma dei pesi
        float totalWeight = 0f;
        foreach (float weight in weights) {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < prefabs.Length; i++) {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight) {
                return prefabs[i];
            }
        }

        // Se non viene selezionato nessun oggetto (cosa che non dovrebbe accadere mai)
        Debug.LogError("[RespawnTrigger.cs] Nessun oggetto selezionato. Controllare i pesi.");
        return null;
    }

    public void ManipulateWeights(int weightIndex, float value, bool positive = true)
    {
        if (positive) {
            weights[weightIndex] = (weights[weightIndex] + value);
        } else {
            weights[weightIndex] = (weights[weightIndex] - value);
        }
    }

    public int GetIndexOfWeightsByTag(string tag)
    {
        switch (tag.ToLower()) {
            case "cherry":
                return 0;
            case "melon":
                return 1;
            case "strawberry":
                return 2;
            case "banana":
                return 3;
            case "hearts":
                return 4;
            case "diamonds":
                return 5;
            case "clubs":
                return 6;
            case "spades":
                return 7;
            case "sun":
                return 8;
            case "moon":
                return 9;
            case "seven":
                return 10;
            case "special":
                return 11;
            case "powerup":
                return 12;               
            default:
                return 0;
        }
    }

    public void ResetWeights()
    {
        weights = GetDefaultWeights();
    }

    public float[] GetWeights()
    {
        return weights;
    }

    public float[] GetDefaultWeights()
    {
        return new float[13] {10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 6f, 6f, 3.8f, 3.7f, 0.5f};
    }

    public void ManipulateSpeed(float speed, int numberOfSpins)
    {
        this.speed = speed;
        numberOfSpecialSpins = sceneManager.GetNumberOfSpins() + numberOfSpins;
    }

    public void SmartBoost(int numberOfSpins)
    {
        float[] defaults = GetDefaultWeights();

        // Check: più di 6 pesi con lo stesso valore e diversi da default -> reset
        Dictionary<float, int> valueCounts = new Dictionary<float, int>();
        foreach (float w in weights)
        {
            float rounded = (float)Math.Round(w, 2);
            if (!valueCounts.ContainsKey(rounded))
                valueCounts[rounded] = 1;
            else
                valueCounts[rounded]++;
        }

        foreach (var kvp in valueCounts)
        {
            float value = kvp.Key;
            int count = kvp.Value;

            int defaultCount = 0;
            foreach (float d in defaults)
            {
                if (Mathf.Approximately(d, value))
                    defaultCount++;
            }

            if (count > 6 && count > defaultCount)
            {
                ResetWeights();
                return;
            }
        }

        // Sblocca index dinamicamente ogni 75 spin
        List<int> candidates = new List<int> { 0, 1, 2, 3 };
        int additional = Mathf.FloorToInt(numberOfSpins / 75f);
        for (int i = 0; i < additional; i++)
        {
            int index = 4 + i;
            if (index < 12) candidates.Add(index);
        }

        // Scegli un candidato random per boost
        int minIndex = candidates[Random.Range(0, candidates.Count)];

        // Boost 20 se index 10 o 11, altrimenti 35
        float boostAmount = (minIndex == 10 || minIndex == 11) ? 20f : 35f;
        weights[minIndex] += boostAmount;

        // Limita il peso massimo a 50
        if (weights[minIndex] > 50f)
            weights[minIndex] = 50f;

        // Trova candidato con peso più alto (escluso minIndex)
        int maxIndex = -1;
        float maxWeight = float.MinValue;
        foreach (int i in candidates)
        {
            if (i != minIndex && weights[i] > maxWeight)
            {
                maxWeight = weights[i];
                maxIndex = i;
            }
        }

        // Abbassa il più alto se sopra default
        if (maxIndex != -1)
        {
            float decreaseAmount = (maxIndex == 10 || maxIndex == 11) ? 18f : 30f;
            weights[maxIndex] = Mathf.Max(weights[maxIndex] - decreaseAmount, defaults[maxIndex]);
        }
    }
}
