using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] prefabs; // Il prefab da istanziare
    public Vector3[] startingPositions; // Array di posizioni
    private GameObject[][] slotCells;

    void Start()
    {
        // Assicurati che l'array di posizioni abbia esattamente 9 elementi
        if (startingPositions.Length != 9) {
            Debug.LogError("Devi specificare esattamente 9 posizioni.");
            return;
        }

        // Inizializza la matrice slotCells come 3x3
        slotCells = new GameObject[3][];
        for (int i = 0; i < slotCells.Length; i++) {
            slotCells[i] = new GameObject[3];
        }

        // Istanzia 9 prefab nelle posizioni specificate
        for (int i = 0; i < startingPositions.Length; i++) {
            Instantiate(GetRandomPrefab(), startingPositions[i], Quaternion.identity);
        }
    }

    public GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    public Vector3[] GetAllStartingPosition()
    {
        return startingPositions;
    }

    public void LogMatrix()
    {
        for (int i = 0; i < slotCells.Length; i++) {
            string row = "";
            for (int j = 0; j < slotCells[i].Length; j++) {
                row += slotCells[i][j] != null ? slotCells[i][j].name : "null";
                row += "\t";
            }
            Debug.Log(row);
        }
    }

    public void AddValueToMatrix(GameObject value, int column)
    {
        // Sposta il contenuto della colonna verso il basso
        ShiftMatrixDown(column);

        if (column < 0 || column >= startingPositions.Length) {
            Debug.LogError("Colonna non valida.");
            return;
        }

        // Aggiungi il valore alla prima riga della colonna specificata
        slotCells[0][column] = value;
    }

    private void ShiftMatrixDown(int column)
    {
        for (int row = slotCells.Length - 1; row > 0; row--) {
            slotCells[row][column] = slotCells[row - 1][column];
        }

        // Elimina il dato nella prima riga
        slotCells[0][column] = null;
    }

    public void RoundPositionByMatrix()
    {
        int positionIndex = 0;

        for (int row = 0; row < slotCells.Length; row++) {
            for (int column = 0; column < slotCells[row].Length; column++) {
                slotCells[row][column].transform.position = startingPositions[positionIndex];
                positionIndex++;
            }
        }
    }
}
