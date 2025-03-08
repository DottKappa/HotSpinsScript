using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
    }

    public void RespawnAtX(float xPosition)
    {
        if (sceneManager == null) {
            Debug.LogError("SceneManager non è stato assegnato.");
            return;
        }

        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        GameObject prefabToSpawn = sceneManager.GetRandomPrefab();
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        SlotController slotController = spawnedObject.GetComponent<SlotController>();
        if (slotController != null) {
            sceneManager.AddValueToMatrix(spawnedObject, GetColumnForMatrix((float)xPosition));
            slotController.SetConstructorValues(false, 23.0f);
        }
    }

// prendere le 3 posizioni dall'array di sceneManager
    int GetColumnForMatrix(float xPosition)
    {
        switch(xPosition)
        {
            case 0f:
                return 0;
            case 3.5f:
                return 1;
            case 6.5f:
                return 2;
            default: {
                Debug.LogError("Posizione non valida. Controllare l'assegnazione della colonna. Possibile che siano state modificate le x");
                return 0;
            }
        }
    }
}
