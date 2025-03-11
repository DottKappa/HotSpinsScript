using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public int counterCell = 0;
    public int counterCherry = 0;
    public int counterMelon = 0;
    public int counterSeven = 0;
    public int counterSpecial = 0;
    private SceneManager sceneManager;
    private float[] weights = new float[5] {20f, 20f, 20f, 20f, 20f};
    private GameObject[] prefabs;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        prefabs = sceneManager.GetAllPrefabs();
    }

    public void RespawnAtX(float xPosition)
    {
        if (sceneManager == null) {
            Debug.LogError("SceneManager non è stato assegnato.");
            return;
        }

        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        GameObject prefabToSpawn = null;

        //CountNewObject(prefabToSpawn); // TODO -> lo potrei usare per qualche trick sulla ruota, o forse no
        
        if (sceneManager.GetNumberOfSpins() < 9) {
            prefabToSpawn = sceneManager.GetRandomPrefab();
        } else {
            prefabToSpawn = GetRandomGameObjectByWeight();
        }

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        SlotController slotController = spawnedObject.GetComponent<SlotController>();
        if (slotController != null) {
            sceneManager.AddValueToMatrix(spawnedObject, GetColumnForMatrix((float)xPosition));
            slotController.SetConstructorValues(false, 23.0f);
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

// deprecated (for now)
    private void CountNewObject(GameObject newObject)
    {
        switch (newObject.tag)
        {
            case string tag when tag.Contains("Cell_"):
                counterCell++;
                break;
            case string tag when tag.Contains("Cherry_"):
                counterCherry++;
                break;
            case string tag when tag.Contains("Melon_"):
                counterMelon++;
                break;
            case string tag when tag.Contains("Seven_"):
                counterSeven++;
                break;
            case string tag when tag.Contains("Special_"):
                counterSpecial++;
                break;
        }
    }
// deprecated (for now)
    private void ResetAllCounter()
    {
        counterCell = counterCherry = counterMelon = counterSeven = counterSpecial = 0;
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
        Debug.LogError("Nessun oggetto selezionato. Controllare i pesi.");
        return null;
    }

    public void ManipulateWeights(int weightIndex, float value)
    {
        weights[weightIndex] = (weights[weightIndex] + value);
    }

    public void ResetWeights()
    {
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = 20f;
        }
    }
}
