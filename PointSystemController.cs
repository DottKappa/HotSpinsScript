using UnityEngine;
using TMPro;
using System.Linq;

public class PointSystemController : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    private int points = 0;
    private SceneManager sceneManager;
    private RespawnTrigger respawnTrigger;
    private FileManager fileManager;
    private CanvasController canvasController;
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

        waifuStepsArray = GetWaifuStepsAsIntegers();
        // Quando avrò già dei dati devo controllare tutto quello che dovrebbe partire allo start (anche le scritte etc)
        points = fileManager.GetPointsByWaifu(fileManager.GetWaifuName());
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

    public void setUpdated(bool value)
    {
        updated = value;
    }

    public void MultipliePoints(int multiplier)
    {
        points *= multiplier;
        UpdatePointsText();
    }

    public void DividePoints(int multiplier)
    {
        points /= multiplier;
        UpdatePointsText();
    }

    public void setCustomMultiplier(float multiplier)
    {
        customMultiplier = multiplier;
    }

    private void checkHorizontal()
    {
        if (slotCells[1][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[1][2].tag) {
            updatePoints(slotCells[1][0].tag.Split('_')[0], 1);
        }
    }

    private void checkDiagonalUpDown()
    {
        if (slotCells[0][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[2][2].tag) {
            updatePoints(slotCells[0][0].tag.Split('_')[0], 2);
        }
    }

    private void checkDiagonalDownUp()
    {
        if (slotCells[2][0].tag == slotCells[1][1].tag && slotCells[1][1].tag == slotCells[0][2].tag) {
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
        if (length % 4 != 0) return points; // Controlla se la lunghezza è un multiplo di 4

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int counter = 0;

        for (int i = length - 1; i >= 0; i--)
        {
            sb.Insert(0, points[i]);
            counter++;
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
        string waifuName = fileManager.GetWaifuName().ToString();
        
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
        WaifuSteps[] steps = (WaifuSteps[])System.Enum.GetValues(typeof(WaifuSteps));
    
        var waifuName = fileManager.GetWaifuName().ToString();
        var filteredSteps = steps.Where(step => step.ToString().Contains(waifuName)).ToArray();
        
        int[][] stepValues = new int[filteredSteps.Length][];

        for (int i = 0; i < filteredSteps.Length; i++) {
            stepValues[i] = new int[2];
            stepValues[i][0] = (int)filteredSteps[i];
            stepValues[i][1] = 0;
        }

        return stepValues;
    }
}
