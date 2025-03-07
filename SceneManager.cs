using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] prefabs; // Il prefab da istanziare
    public Vector3[] positions; // Array di posizioni

    void Start()
    {
        // Assicurati che l'array di posizioni abbia esattamente 9 elementi
        if (positions.Length != 9) {
            Debug.LogError("Devi specificare esattamente 9 posizioni.");
            return;
        }

        // Istanzia 9 prefab nelle posizioni specificate
        for (int i = 0; i < positions.Length; i++) {
            Instantiate(GetRandomPrefab(), positions[i], Quaternion.identity);
        }
    }

    public GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    public Vector3[] GetAllStartingPosition()
    {
        return positions;
    }
}
