using UnityEngine;
using TMPro;

public class PointSystemController : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    private int points = 0;
    private SceneManager sceneManager;
    private RespawnTrigger respawnTrigger;
    private GameObject[][] slotCells;
    private bool updated = false;
    private float customMultiplier = 1f;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        respawnTrigger = FindFirstObjectByType<RespawnTrigger>();
    }

    public void FetchPoints()
    {
        if (!updated) {
            slotCells = sceneManager.GetMatrix();
            checkHorizontal();
            checkDiagonalUpDown();
            checkDiagonalDownUp();
            UpdatePointsText();
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
        points = (int)calculatePoints;

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
}
