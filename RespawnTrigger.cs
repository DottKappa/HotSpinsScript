using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private SceneManager sceneManager;
    private float[] weights = new float[6] {20f, 20f, 20f, 20f, 20f, 1000.1f};
    private GameObject[] prefabs;
    private float speed = 23.0f;
    private int numberOfSpecialSpins = 0;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
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
        switch (tag.ToLower())
        {
            case "cell":
                return 0;
            case "cherry":
                return 1;
            case "melon":
                return 2;
            case "seven":
                return 3;
            case "special":
                return 4;
            case "powerup":
                return 5;
            default:
                return 0;
        }
    }

    public void ResetWeights()
    {
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = 20f;
        }
        weights[weights.Length-1] = 0.1f;
    }

    public void ManipulateSpeed(float speed, int numberOfSpins)
    {
        this.speed = speed;
        numberOfSpecialSpins = sceneManager.GetNumberOfSpins() + numberOfSpins;
    }
}
