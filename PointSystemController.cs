using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PointSystemController : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    private int points = 0;
    private SceneManager sceneManager;
    private RespawnTrigger respawnTrigger;
    private FileManager fileManager;
    private CanvasController canvasController;
    private VfxManager vfxManager;
    private CameraSlot cameraSlot;
    private GameObject[][] slotCells;
    private bool updated = false;
    private float customMultiplier = 1f;
    private int numberOfSpinToBuff = 0;
    private int numberOfSpinToDebuff = 0;
    private int[][] waifuStepsArray;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        respawnTrigger = FindFirstObjectByType<RespawnTrigger>();
        fileManager = FindFirstObjectByType<FileManager>();
        canvasController = FindFirstObjectByType<CanvasController>();
        vfxManager = FindFirstObjectByType<VfxManager>(); 
        cameraSlot = FindFirstObjectByType<CameraSlot>();

        waifuStepsArray = GetWaifuStepsAsIntegers();

        points = fileManager.GetPointsByWaifu(fileManager.GetActiveWaifuName());
        UpdatePointsText();
        UpdateWaifuImage();
    }

    public void FetchPoints()
    {
        if (!updated) {
            slotCells = sceneManager.GetMatrix();
            checkHorizontal();
            checkDiagonalUpDown();
            checkDiagonalDownUp();
            UpdatePointsText();
            UpdateWaifuImage();
            updated = true;
        }
    }

    public int GetPoints()
    {
        return points;
    }

    public void setUpdated(bool value)
    {
        updated = value;
    }

    public void MultipliePoints(int multiplier)
    {
        points *= multiplier;
        UpdatePointsText();
        UpdateWaifuImage();
    }

    public void DividePoints(int multiplier)
    {
        points /= multiplier;
        UpdatePointsText();
        UpdateWaifuImage();
    }

    public void setCustomMultiplier(float multiplier)
    {
        customMultiplier = multiplier;
    }

    private void checkHorizontal()
    {
        if (slotCells[1][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[1][2].tag) {
            callAnimationThunder("H");
            callWinSound();
            callAnimationCell(slotCells[1][0]);
            callAnimationCell(slotCells[1][1]);
            callAnimationCell(slotCells[1][2]);
            updatePoints(slotCells[1][0].tag.Split('_')[0], 1);
        }
    }

    private void checkDiagonalUpDown()
    {
        if (slotCells[0][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[2][2].tag) {
            callAnimationThunder("U");
            callWinSound();
            callAnimationCell(slotCells[0][0]);
            callAnimationCell(slotCells[1][1]);
            callAnimationCell(slotCells[2][2]);
            updatePoints(slotCells[0][0].tag.Split('_')[0], 2);
        }
    }

    private void checkDiagonalDownUp()
    {
        if (slotCells[2][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[0][2].tag) {
            callAnimationThunder("D");
            callWinSound();
            callAnimationCell(slotCells[2][0]);
            callAnimationCell(slotCells[1][1]);
            callAnimationCell(slotCells[0][2]);
            updatePoints(slotCells[2][0].tag.Split('_')[0], 2);
        }
    }

    private void updatePoints(string tag, int multiplier)
    {
        float calculatePoints = points;
        if (System.Enum.TryParse(tag, out SlotSymbols symbol)) {
            int value = (int)symbol;
            calculatePoints += value * multiplier * customMultiplier;
        } else {
            calculatePoints += 1 * multiplier * customMultiplier;
        }
        points = ManipulateWinWithPowerUp(calculatePoints);

        respawnTrigger.ResetWeights();
    }

    private void UpdatePointsText()
    {
        int fakeAnimation = 5;
        while (fakeAnimation != 0) {
            pointText.text = addDot((points / fakeAnimation).ToString());
            fakeAnimation--;
        }
    }

    private string addDot(string points)
    {
        int length = points.Length;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int counter = 0;

        // Partiamo dal termine della stringa e aggiungiamo i punti ogni 3 caratteri
        for (int i = length - 1; i >= 0; i--)
        {
            sb.Insert(0, points[i]);
            counter++;

            // Aggiungi un punto ogni 3 caratteri, ma non alla fine
            if (counter == 3 && i != 0)
            {
                sb.Insert(0, '.');
                counter = 0;
            }
        }

        return sb.ToString();
    }

    private int ManipulateWinWithPowerUp(float points)
    {
        if (numberOfSpinToBuff != 0 && sceneManager.GetNumberOfSpins() % numberOfSpinToBuff == 0) {
            return (int)(points * 2);
        } else if (numberOfSpinToDebuff != 0 && sceneManager.GetNumberOfSpins() % numberOfSpinToDebuff == 0) {
            return (int)(points / 2);
        }

        return (int)points;
    }

    public void SetNumberOfSpinToBuff(int number)
    {
        numberOfSpinToBuff = number;
    }

    public void SetNumberOfSpinToDebuff(int number)
    {
        numberOfSpinToDebuff = number;
    }

    private void UpdateWaifuImage()
    {
        string waifuName = fileManager.GetActiveWaifuName().ToString();
        for (int i = 0; i < waifuStepsArray.Length; i++) {
            if (points >= waifuStepsArray[i][0] && waifuStepsArray[i][1] == 0) {
                canvasController.SetWaifuImage(waifuName, waifuName + "_" + (i + 1));
                waifuStepsArray[i][1] = 1;
            } else if (i + 1 < waifuStepsArray.Length && points < waifuStepsArray[i + 1][0]) {
                break;
            }
        }
    }

    private int[][] GetWaifuStepsAsIntegers()
    {
        var waifuName = fileManager.GetActiveWaifuName().ToString();
        int actualStep = fileManager.GetImageStepByWaifu(fileManager.GetActiveWaifuName());
        int i = 1;
        bool nextStepExist = true;
        int stepValueLength = 0;
        
        while (nextStepExist) {
            string index = i.ToString();
            if (Enum.TryParse(waifuName+"_"+index, out WaifuSteps waifuStep)) {
                stepValueLength++;
                i++;
            } else {
                nextStepExist = false;
                break;
            }
        }

        i = 1;
        nextStepExist = true;
        int[][] stepValues = new int[stepValueLength][];

        while (nextStepExist) {
            string index = i.ToString();
            if (Enum.TryParse(waifuName+"_"+index, out WaifuSteps waifuStep)) {
                stepValues[i - 1] = new int[2];
                stepValues[i - 1][0] = (int)waifuStep;
                // Metto true solo se l'ho sorpassato e sono a quello dopo
                stepValues[i - 1][1] = (i < actualStep) ? 1 : 0;
                i++;
            } else {
                nextStepExist = false;
                break;
            }
        }

        return stepValues;
    }

    public int GetActualImageStep()
    {
        for (int i = 0; i < waifuStepsArray.Length; i++) {
            // Per ottenere quello attivo mi basta prendere la posizione del primo non attivo (i è indietro di 1)
            if (waifuStepsArray[i][1] != 1) {
                return i;
            }
        }
        
        return waifuStepsArray.Length;
    }

    public int GetPointsForNextArt()
    {
        if (waifuStepsArray != null && waifuStepsArray.Length > 0) {
            for (int i = 0; i < waifuStepsArray.Length; i++) {
                if (waifuStepsArray[i][1] == 0) {
                    if (i + 1 >= waifuStepsArray.Length) {
                        return -999; // Vuol dire che ho finito le art
                    } else {
                        return waifuStepsArray[i][0];
                    }                    
                }
            }

            return -999;
        } 

        var waifuSteps = Enum.GetValues(typeof(WaifuSteps))
                            .Cast<WaifuSteps>()
                            .Where(e => e.ToString().ToLower().Contains(fileManager.GetActiveWaifuName().ToString().ToLower()))
                            .ToList();
        if (waifuSteps.Count >= 1) {
            return (int)waifuSteps[0];
        } 

        return -1; // non ci sono abbastanza elementi con "ActiveWaifuName"    
    }

    private void callAnimationCell(GameObject cell)
    {
        SlotController slotControllerScript = cell.GetComponent<SlotController>();
        if (slotControllerScript != null) {
            slotControllerScript.IncreaseScaleGradually();
        } else {
            Debug.LogError("[PointSystemController.cs] C'è un errore nel chiamare l'animazione della cella");
        }
    }

    private void callAnimationThunder(string rotation)
    {
        vfxManager.PlayThunder(rotation);
    }

    private void callWinSound()
    {
        cameraSlot.StartNormalWinSound();
        canvasController.ShakeSlot();
    }
}
