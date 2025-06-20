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
    private PointSystemIdleController pointSystemIdleController;
    private SelectorSkin selectorSkin;
    private GameObject[][] slotCells;
    private bool updated = false;
    private int numberOfSpinToBuff = 0;
    private int numberOfSpinToDebuff = 0;
    private int[][] waifuStepsArray;
    private bool isHorizontalUpActive = false;
    private bool isHorizontalDownActive = false;
    private bool isVerticalLeftActive = false;
    private bool isVerticalRightActive = false;
    private int limitPoints = 9999999;
    private int lastWin = 0;
    public int GetLastWin() => lastWin;
    public void IncrementLastWin() => lastWin++;
    public void ResetLastWin() => lastWin = 0;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        respawnTrigger = FindFirstObjectByType<RespawnTrigger>();
        fileManager = FindFirstObjectByType<FileManager>();
        canvasController = FindFirstObjectByType<CanvasController>();
        vfxManager = FindFirstObjectByType<VfxManager>();
        cameraSlot = FindFirstObjectByType<CameraSlot>();
        pointSystemIdleController = GetComponent<PointSystemIdleController>();
        selectorSkin = FindFirstObjectByType<SelectorSkin>();

        waifuStepsArray = GetWaifuStepsAsIntegers();

        points = fileManager.GetPointsByWaifu(fileManager.GetActiveWaifuName());
        UpdatePointsText();
        UpdateWaifuImage();
    }

    public void FetchPoints()
    {
        if (!updated)
        {
            slotCells = sceneManager.GetMatrix();
            checkHorizontal();
            if (isHorizontalUpActive)
            {
                checkHorizontal(0, "HU");
            }
            if (isHorizontalDownActive)
            {
                checkHorizontal(2, "HD");
            }
            checkDiagonalUpDown();
            checkDiagonalDownUp();
            if (isVerticalLeftActive)
            {
                checkVertical(0, "VL");
            }
            if (isVerticalRightActive)
            {
                checkVertical(2, "VR");
            }
            UpdatePointsText();
            UpdateWaifuImage();
            updated = true;
            if (pointSystemIdleController.GetNumberOfHorizontal() > 0) pointSystemIdleController.SetNumberOfHorizontal(-1);
            if (pointSystemIdleController.GetNumberOfUpDown() > 0) pointSystemIdleController.SetNumberOfUpDown(-1);
            if (pointSystemIdleController.GetNumberOfDownUp() > 0) pointSystemIdleController.SetNumberOfDownUp(-1);
            UpdateIdleMultiplierByController();
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

    public void SetHorizontalUp(bool value)
    {
        isHorizontalUpActive = value;
    }

    public void SetHorizontalDown(bool value)
    {
        isHorizontalDownActive = value;
    }

    public void SetVerticalLeft(bool value)
    {
        isVerticalLeftActive = value;
    }

    public void SetVerticalRight(bool value)
    {
        isVerticalRightActive = value;
    }

    public void MultipliePoints(float multiplier)
    {
        points = Mathf.FloorToInt(points * multiplier);
        if (points >= limitPoints) points = limitPoints;

        UpdatePointsText();
        UpdateWaifuImage();
        selectorSkin.SetUpImageButtons();
    }

    public void DividePoints(float multiplier)
    {
        points = Mathf.FloorToInt(points / multiplier);
        if (points <= 0) points = 0;

        RecalculateWaifuSteps();
        UpdatePointsText();
        UpdateWaifuImage();
        selectorSkin.SetUpImageButtons();
    }

    private void checkHorizontal(int row = 1, string animationPosition = "H")
    {
        if (slotCells[row][0].tag == slotCells[row][1].tag && slotCells[row][1].tag == slotCells[row][2].tag)
        {
            int multiplier = 1;
            callAnimation(animationPosition);
            callWinSound();
            callAnimationCell(slotCells[row][0]);
            callAnimationCell(slotCells[row][1]);
            callAnimationCell(slotCells[row][2]);
            if (pointSystemIdleController.GetNumberOfHorizontal() > 0)
            {
                multiplier = pointSystemIdleController.GetHorizontalMultiplier();
            }
            updatePoints(slotCells[row][0].tag.Split('_')[0], multiplier);
        }
    }

    private void checkDiagonalUpDown()
    {
        if (slotCells[0][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[2][2].tag)
        {
            int multiplier = 2;
            callAnimation("U");
            callWinSound();
            callAnimationCell(slotCells[0][0]);
            callAnimationCell(slotCells[1][1]);
            callAnimationCell(slotCells[2][2]);
            if (pointSystemIdleController.GetNumberOfUpDown() > 0)
            {
                multiplier = pointSystemIdleController.GetUpDownlMultiplier();
            }
            updatePoints(slotCells[0][0].tag.Split('_')[0], multiplier);
        }
    }

    private void checkDiagonalDownUp()
    {
        if (slotCells[2][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[0][2].tag)
        {
            int multiplier = 2;
            callAnimation("D");
            callWinSound();
            callAnimationCell(slotCells[2][0]);
            callAnimationCell(slotCells[1][1]);
            callAnimationCell(slotCells[0][2]);
            if (pointSystemIdleController.GetNumberOfDownUp() > 0)
            {
                multiplier = pointSystemIdleController.GetDownUpMultiplier();
            }
            updatePoints(slotCells[2][0].tag.Split('_')[0], multiplier);
        }
    }

    private void checkVertical(int column, string animationPosition = "")
    {
        if (slotCells[0][column].tag == slotCells[1][column].tag && slotCells[1][column].tag == slotCells[2][column].tag)
        {
            int multiplier = 1;
            callAnimation(animationPosition);
            callWinSound();
            callAnimationCell(slotCells[0][column]);
            callAnimationCell(slotCells[1][column]);
            callAnimationCell(slotCells[2][column]);
            updatePoints(slotCells[0][column].tag.Split('_')[0], multiplier);
        }
    }

    private void updatePoints(string tag, int multiplier)
    {
        float calculatePoints = 0;
        if (System.Enum.TryParse(tag, out SlotSymbols symbol))
        {
            int value = (int)symbol;
            calculatePoints += value * multiplier;
        }
        else
        {
            calculatePoints += 1 * multiplier;
        }

        int actualWin = ManipulateWinWithPowerUp(calculatePoints);
        if ((actualWin) > 10000)
        {
            callCoinEmitter(actualWin);
        }
        points = actualWin + points;

        if (points >= limitPoints)
        {
            points = limitPoints;
        }

        ResetLastWin();
        //respawnTrigger.ResetWeights(); -> Non mi interessa più resettarli
    }

    private void UpdatePointsText()
    {
        int fakeAnimation = 5;
        while (fakeAnimation != 0)
        {
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
        if (numberOfSpinToBuff != 0 && sceneManager.GetNumberOfSpins() % numberOfSpinToBuff == 0)
        {
            return (int)(points * 2);
        }
        else if (numberOfSpinToDebuff != 0 && sceneManager.GetNumberOfSpins() % numberOfSpinToDebuff == 0)
        {
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
        for (int i = 0; i < waifuStepsArray.Length; i++)
        {
            if (points >= waifuStepsArray[i][0] && waifuStepsArray[i][1] == 0)
            {
                waifuStepsArray[i][1] = 1;
                canvasController.SetWaifuImage(waifuName, waifuName + "_" + (i + 1));
            }
            else if (i + 1 < waifuStepsArray.Length && points < waifuStepsArray[i + 1][0])
            {
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

        while (nextStepExist)
        {
            string index = i.ToString();
            if (Enum.TryParse(waifuName + "_" + index, out WaifuSteps waifuStep))
            {
                stepValueLength++;
                i++;
            }
            else
            {
                nextStepExist = false;
                break;
            }
        }

        i = 1;
        nextStepExist = true;
        int[][] stepValues = new int[stepValueLength][];

        while (nextStepExist)
        {
            string index = i.ToString();
            if (Enum.TryParse(waifuName + "_" + index, out WaifuSteps waifuStep))
            {
                stepValues[i - 1] = new int[2];
                stepValues[i - 1][0] = (int)waifuStep;
                // Metto true solo se l'ho sorpassato e sono a quello dopo
                stepValues[i - 1][1] = (i < actualStep) ? 1 : 0;
                i++;
            }
            else
            {
                nextStepExist = false;
                break;
            }
        }

        return stepValues;
    }

    public int GetActualImageStep()
    {
        for (int i = 0; i < waifuStepsArray.Length; i++)
        {
            // Per ottenere quello attivo mi basta prendere la posizione del primo non attivo (i è indietro di 1)
            if (waifuStepsArray[i][1] != 1)
            {
                return i;
            }
        }

        return waifuStepsArray.Length;
    }

    public int GetPointsForNextArt()
    {
        if (waifuStepsArray != null && waifuStepsArray.Length > 0)
        {
            for (int i = 0; i < waifuStepsArray.Length; i++)
            {
                if (waifuStepsArray[i][1] == 0)
                {
                    if (i >= waifuStepsArray.Length)
                    {
                        return -999; // Vuol dire che ho finito le art
                    }
                    else
                    {
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
        if (waifuSteps.Count >= 1)
        {
            return (int)waifuSteps[0];
        }

        return -1; // non ci sono abbastanza elementi con "ActiveWaifuName"    
    }

    private void callAnimationCell(GameObject cell)
    {
        SlotController slotControllerScript = cell.GetComponent<SlotController>();
        if (slotControllerScript != null)
        {
            slotControllerScript.IncreaseScaleGradually();
        }
        else
        {
            Debug.LogError("[PointSystemController.cs] C'è un errore nel chiamare l'animazione della cella");
        }
    }

    private void callAnimation(string rotation)
    {
        vfxManager.PlayVfx(rotation);
    }

    private void callCoinEmitter(int winPoints)
    {
        int emission = 1;
        if (winPoints > 20000 && winPoints <= 30000)
        {
            emission = 2;
        }
        else if (winPoints > 30000)
        {
            emission = 3;
        }

        vfxManager.PlayCoinEmitter(emission);
    }

    private void callWinSound()
    {
        cameraSlot.StartNormalWinSound();
        canvasController.ShakeSlot();
    }

    private void RecalculateWaifuSteps()
    {
        if (waifuStepsArray == null || waifuStepsArray.Length == 0) return;

        string waifuName = fileManager.GetActiveWaifuName().ToString();
        int lastUnlockedIndex = -1;

        for (int i = 0; i < waifuStepsArray.Length; i++)
        {
            if (points >= waifuStepsArray[i][0])
            {
                waifuStepsArray[i][1] = 1;
                lastUnlockedIndex = i;
            }
            else
            {
                waifuStepsArray[i][1] = 0;
            }
        }

        Waifu activeWaifu = fileManager.GetActiveWaifuName();
        // Aggiorna immagine waifu allo step più recente disponibile
        if (lastUnlockedIndex >= 0)
        {
            string stepName = waifuName + "_" + (lastUnlockedIndex + 1);
            canvasController.SetWaifuImage(waifuName, stepName);

            // Salva step corrente nel file
            fileManager.SetImageStepByWaifu(lastUnlockedIndex + 1, activeWaifu);
            fileManager.SaveWaifuFile();
        }
        else
        {
            // Se nessuno step è attivo, resetta a step 0
            fileManager.SetImageStepByWaifu(0, activeWaifu);
            fileManager.SaveWaifuFile();
        }
    }

    public void UpdateIdleMultiplierByController()
    {
        pointSystemIdleController.UpdateIdleMultipliers();
    }
}
