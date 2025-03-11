using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] prefabs; // Il prefab da istanziare
    public Vector3[] startingPositions; // Array di posizioni
    private GameObject[][] slotCells;
    private bool isRolling = false;
    private int numberOfSpins = 0;
    private bool[] isRollingByColumn = new bool[3] {true, true, true};
    private PointSystemController pointSystemController;
    private RespawnTrigger respawnTrigger;

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

        pointSystemController = FindFirstObjectByType<PointSystemController>();
        respawnTrigger = FindFirstObjectByType<RespawnTrigger>();
    }

    void Update()
    {
        if (!MatrixHasEmptySlot() || !isRolling) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (!isRolling) {
                    EmptySlotMatrix();
                    StartSlot();
                    pointSystemController.setUpdated(false);
                    isRolling = true;
                    isRollingByColumn = new bool[3] {true, true, true};
                    numberOfSpins++;
                } else {
                    StopSlot();
                    isRolling = false;
                    isRollingByColumn = new bool[3] {false, false, false};
                }
            }

            if (isRollingByColumn[0] || isRollingByColumn[1] || isRollingByColumn[2]) {
                StopSlotByColumn();
                pointSystemController.setUpdated(false);
            }
            if (!isRollingByColumn[0] && !isRollingByColumn[1] && !isRollingByColumn[2]) {
                isRolling = false;
            }

            if (!isRollingByColumn[0] && !isRollingByColumn[1] && !isRollingByColumn[2] && !isRolling) {
                pointSystemController.FetchPoints();
            }
        }
    }

    public GameObject[] GetAllPrefabs()
    {
        return prefabs;
    }

    public GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    public GameObject GetCellPrefab()
    {
        return prefabs[0];
    }
    
    public GameObject GetCherryPrefab()
    {
        return prefabs[1];
    }
    
    public GameObject GetMelonPrefab()
    {
        return prefabs[2];
    }
    
    public GameObject GetSevenPrefab()
    {
        return prefabs[3];
    }
    
    public GameObject GetSpecialPrefab()
    {
        return prefabs[4];
    }

    public Vector3[] GetAllStartingPosition()
    {
        return startingPositions;
    }

    public GameObject[][] GetMatrix()
    {
        return slotCells;
    }

    public int GetNumberOfSpins()
    {
        return numberOfSpins;
    }

    public void StartSlot()
    {
        // Istanzia 9 prefab nelle posizioni specificate
        for (int i = 0; i < startingPositions.Length; i++) {
            Instantiate(GetRandomPrefab(), startingPositions[i], Quaternion.identity);
        }

        switch (numberOfSpins) {
            case int n when n % 3 == 0:
            respawnTrigger.ManipulateWeights(0, 4f);
            break;
            case int n when n % 5 == 0:
            respawnTrigger.ManipulateWeights(0, -10f);
            respawnTrigger.ManipulateWeights(1, -2f);
            break;
            case int n when n % 7 == 0:
            respawnTrigger.ManipulateWeights(2, 10f);
            break;
            case int n when n % 11 == 0:
            respawnTrigger.ManipulateWeights(1, 3f);
            respawnTrigger.ManipulateWeights(2, -3f);
            break;
            case int n when n % 13 == 0:
            respawnTrigger.ManipulateWeights(4, 20f);
            break;
        }
    }

    public void StopSlot()
    {
        SlotController[] slotControllers = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        foreach (SlotController slotController in slotControllers) {
            slotController.SetMoving(false);
        }

        RoundPositionByMatrix();
    }

    private void EmptySlotMatrix()
    {
        for (int i = 0; i < slotCells.Length; i++) {
            for (int j = 0; j < slotCells[i].Length; j++) {
                if (slotCells[i][j] != null) {
                    Destroy(slotCells[i][j]);
                    slotCells[i][j] = null;
                }
            }
        }
    }

    public void StopSlotByColumn()
    {
        int column = -1;

        if (Input.GetKeyDown(KeyCode.S)) {
            column = 0;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            column = 1;
        } else if (Input.GetKeyDown(KeyCode.F)) {
            column = 2;
        }

        if (column != -1) {
            for (int row = 0; row < slotCells.Length; row++) {
                if (slotCells[row][column] != null) {
                    slotCells[row][column].GetComponent<SlotController>().SetMoving(false);
                    RoundPositionByColumn(column);
                    isRollingByColumn[column] = false;
                }
            }
        }
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

    private bool MatrixHasEmptySlot()
    {
        for (int i = 0; i < slotCells.Length; i++) {
            for (int j = 0; j < slotCells[i].Length; j++) {
                if (slotCells[i][j] == null) {
                    return true;
                }
            }
        }

        return false;
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

    public void RoundPositionByColumn(int column)
    {
        int[] positions = new int[] { 0, 1, 2 };

        switch (column) {
            case 0: {
                positions = new int[] { 0, 3, 6 };
                break;
            }
            case 1: {
                positions = new int[] { 1, 4, 7 };
                break;
            }
            case 2: {
                positions = new int[] { 2, 5, 8 };
                break;
            }
        }

        for (int row = 0; row < slotCells.Length; row++) {
            slotCells[row][column].transform.position = startingPositions[positions[row]];
        }
    }
}
