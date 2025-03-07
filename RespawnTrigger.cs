using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private SceneManager sceneManager;
    private int respawnCount = 0;

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
            slotController.SetConstructorValues(false, 23.0f);
        }
    }
}
