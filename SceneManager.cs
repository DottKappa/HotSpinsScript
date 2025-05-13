using UnityEngine;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public GameObject[] prefabs; // Il prefab da istanziare
    public Vector3[] startingPositions; // Array di posizioni
    private GameObject[][] slotCells;
    private bool isRolling = false;
    private int numberOfSpins = 0;
    private bool[] isRollingByColumn = new bool[3] {true, true, true};
    private bool needSave = true;
    private PointSystemController pointSystemController;
    private RespawnTrigger respawnTrigger;
    private PowerUpManager powerUpManager;
    private FileManager fileManager;
    private BuffDebuffManager buffDebuffManager;
    private CameraSlot cameraSlot;
    private IdleFileManager idleFileManager;

    // Gestione Bug colonne
    private bool isBusy = false;
    public bool IsBusy() => isBusy;
    public void SetBusy(bool value) => isBusy = value;

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
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
        fileManager = FindFirstObjectByType<FileManager>();
        buffDebuffManager = FindFirstObjectByType<BuffDebuffManager>();
        cameraSlot = FindFirstObjectByType<CameraSlot>();
        idleFileManager = FindFirstObjectByType<IdleFileManager>();
        
        numberOfSpins = fileManager.GetSpinsByWaifu(fileManager.GetActiveWaifuName());
    }

    void Update()
    {
        if (!MatrixHasEmptySlot() || !isRolling) {
            if (Input.GetKeyDown(KeyCode.Space) && !IsBusy()) {
                if (!isRolling) {
                    StartSlot();                    
                } else {
                    StopSlot();
                }
            }

            if (!IsBusy() && isRollingByColumn[0] || isRollingByColumn[1] || isRollingByColumn[2]) {
                StopSlotByColumn();
                pointSystemController.setUpdated(false);
            }
            if (!isRollingByColumn[0] && !isRollingByColumn[1] && !isRollingByColumn[2]) {
                isRolling = false;
            }
            if (!isRollingByColumn[0] && !isRollingByColumn[1] && !isRollingByColumn[2] && !isRolling) {
                cameraSlot.StopSlotSpinSound();
                pointSystemController.FetchPoints();
                
                if (needSave) {
                    saveWaifuData();
                    needSave = false;
                }
            }
        }

        if (!MatrixHasEmptySlot() && !isRolling) {
            if (Input.GetKeyDown(KeyCode.W)) {
                PickUpSpark();
            }
        }

        // Torno al menù principale
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PlayerPrefs.SetInt("skipWelcomePage", 1);
            idleFileManager.SaveIdleFile();
            saveWaifuData();
            SceneManagement.LoadScene("Menu");
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

    public bool GetIsRolling()
    {
        return isRolling;
    }

    public int GetNumberOfSparksInSlot()
    {
        int numberOfSparks = 0;
        
        if (!MatrixHasEmptySlot() && !isRolling) {
            for (int i = 0; i < slotCells.Length; i++) {
                for (int j = 0; j < slotCells[i].Length; j++) {
                    if (slotCells[i][j] != null && slotCells[i][j].tag.Contains("Powerup_")) {
                        numberOfSparks++;
                    }
                }
            }
        }

        return numberOfSparks;
    }

    public void StartSlot()
    {
        cameraSlot.StartSlotSpinSound();
        EmptySlotMatrix();
        needSave = true;

        // Istanzia 9 prefab nelle posizioni specificate
        for (int i = 0; i < startingPositions.Length; i++) {
            Instantiate(GetRandomPrefab(), startingPositions[i], Quaternion.identity);
        }

        ManipulateSlot();

        pointSystemController.setUpdated(false);
        isRolling = true;
        isRollingByColumn = new bool[3] {true, true, true};
        numberOfSpins++;
    }

    public void StopSlot()
    {
        SlotController[] slotControllers = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        foreach (SlotController slotController in slotControllers) {
            slotController.SetMoving(false);
        }
        
        isRolling = false;
        isRollingByColumn = new bool[3] {false, false, false};

        if (GetNumberOfSparksInSlot() > 0) {
            cameraSlot.StartSparkInSlotSound();
        }
        RoundPositionByMatrix();
        StartCoroutine(StopSlotWithDelay());
    }

    private IEnumerator StopSlotWithDelay()
    {
        StopSlotByColumn(SlotColumns.First);
        cameraSlot.StopSpinSound();
        yield return new WaitForSeconds(0.08f);

        StopSlotByColumn(SlotColumns.Second);
        cameraSlot.StopSpinSound();
        yield return new WaitForSeconds(0.08f);

        StopSlotByColumn(SlotColumns.Third);
        cameraSlot.StopSpinSound();
    }

    public void EmptySlotMatrix()
    {
        for (int i = 0; i < slotCells.Length; i++) {
            for (int j = 0; j < slotCells[i].Length; j++) {
                if (slotCells[i][j] != null) {
                    Destroy(slotCells[i][j]);
                    slotCells[i][j] = null;
                }
            }
        }

        // Cancellare se diventa troppo pesante
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects) {
            if (obj != null &&
                obj.name.Contains("(Clone)") &&
                obj.tag.Contains("_SlotCell"))
            {
                Destroy(obj);
            }
        }
    }

    public void StopSlotByColumn(SlotColumns? column = null)
    {
        if (column == null) {
            if (Input.GetKeyDown(KeyCode.A)) {
                column = SlotColumns.First;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                column = SlotColumns.Second;
            } else if (Input.GetKeyDown(KeyCode.D)) {
                column = SlotColumns.Third;
            }
        }

        if (column == SlotColumns.First && isRollingByColumn[0]) {
            cameraSlot.StopSpinSound();
        } else if (column == SlotColumns.Second && isRollingByColumn[1]) {
            cameraSlot.StopSpinSound();
        } else if (column == SlotColumns.Third && isRollingByColumn[2]) {
            cameraSlot.StopSpinSound();
        }

        if (column != null) {
            int colIndex = (int)column;
            for (int row = 0; row < slotCells.Length; row++) {
                if (slotCells[row][colIndex] != null) {
                    slotCells[row][colIndex].GetComponent<SlotController>().SetMoving(false);
                    RoundPositionByColumn(colIndex);
                    isRollingByColumn[colIndex] = false;
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

    public bool MatrixHasEmptySlot()
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

// TODO: Deprecated
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
            Vector3 finalPosition = startingPositions[positions[row]];

            if (row == (slotCells.Length - 1)) {
                slotCells[row][column].GetComponent<SlotController>().StartDropAndRise(finalPosition, 0.3f, 15f);
            } else {
                slotCells[row][column].transform.position = finalPosition;
            }
        }
    }

    private void ManipulateSlot()
    {
        switch (numberOfSpins) {
            case int n when n % 3 == 0:
            respawnTrigger.ManipulateWeights(4, 4f);
            break;
            case int n when n % 5 == 0:
            respawnTrigger.ManipulateWeights(5, -4f);
            respawnTrigger.ManipulateWeights(4, -3f);
            break;
            case int n when n % 7 == 0:
            respawnTrigger.ManipulateWeights(6, 5f);
            break;
            case int n when n % 11 == 0:
            respawnTrigger.ManipulateWeights(7, 3f);
            respawnTrigger.ManipulateWeights(6, -3f);
            break;
            case int n when n % 13 == 0:
            respawnTrigger.ManipulateWeights(5, 10f);
            break;
        }
    }

    public void PickUpSpark()
    {
        int numberOfSparksInSlot = GetNumberOfSparksInSlot();
        if (numberOfSparksInSlot > 0) {
            EmptySlotMatrix();
            idleFileManager.UpdateNumberOfUnlockableRoom(numberOfSparksInSlot);
            powerUpManager.addSpark(numberOfSparksInSlot);
        }
    }

    private void saveWaifuData()
    {
        Waifu waifuName = (Waifu)System.Enum.Parse(typeof(Waifu), PlayerPrefs.GetString("waifuName"));

        fileManager.SetPointsByWaifu(pointSystemController.GetPoints(), waifuName);
        fileManager.SetSpinsByWaifu(numberOfSpins, waifuName);
        fileManager.SetImageStepByWaifu(pointSystemController.GetActualImageStep(), waifuName);
        fileManager.SetBuffUsedByWaifu(System.Enum.GetNames(typeof(BuffType)), buffDebuffManager.GetIsUsedByDictionary(true), waifuName);
        fileManager.SetDebuffUsedByWaifu(System.Enum.GetNames(typeof(DebuffType)), buffDebuffManager.GetIsUsedByDictionary(false), waifuName);
        fileManager.SetWeightsByWaifu(respawnTrigger.GetWeights(), waifuName);

        Debug.Log("[SceneManager] Salvataggio dati waifu");
        fileManager.SaveWaifuFile();
    }

    private void OnApplicationQuit()
    {
        saveWaifuData();
    }
}
