using UnityEngine;
using TMPro;

public class PointSystemIdleController : MonoBehaviour
{
    [Header("Multiplier text")]
    public TextMeshProUGUI upDownText;
    public TextMeshProUGUI horizontalText;
    public TextMeshProUGUI downUpText;
    [Header("Multiplier usage")]
    public TextMeshProUGUI upDownUsage;
    public TextMeshProUGUI horizontalUsage;
    public TextMeshProUGUI downUpUsage;

    private int numberOfHorizontal = 0;
    private int numberOfUpDown = 0;
    private int numberOfDownUp = 0;
    private int horizontalMultiplier = 1;
    private int upDownMultiplier = 2;
    private int downUpMultiplier = 2;

    private FileManager fileManager;

// == SET
    public void SetNumberOfHorizontal(int value) { numberOfHorizontal += value; }
    public void SetNumberOfUpDown(int value) { numberOfUpDown += value; }
    public void SetNumberOfDownUp(int value) { numberOfDownUp += value; }
    public void SetHorizontalMultiplier(int multiplier) { horizontalMultiplier = horizontalMultiplier * multiplier; }
    public void SetUpDownMultiplier(int multiplier) { upDownMultiplier = upDownMultiplier * multiplier; }
    public void SetDownUpMultiplier(int multiplier) { downUpMultiplier = downUpMultiplier * multiplier; }

// == GET
    public int GetNumberOfHorizontal() { return numberOfHorizontal; }
    public int GetNumberOfUpDown() { return numberOfUpDown; }
    public int GetNumberOfDownUp() { return numberOfDownUp; }
    public int GetHorizontalMultiplier() { return horizontalMultiplier; }
    public int GetUpDownlMultiplier() { return upDownMultiplier; }
    public int GetDownUpMultiplier() { return downUpMultiplier; }

    void Start()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        Multiplier multiplier = fileManager.GetMultiplierByWaifu(fileManager.GetActiveWaifuName());
        horizontalMultiplier = multiplier.GetHorizontal().GetValue();
        numberOfHorizontal = multiplier.GetHorizontal().GetUsesLeft();
        upDownMultiplier = multiplier.GetUpDown().GetValue();
        numberOfUpDown = multiplier.GetUpDown().GetUsesLeft();
        downUpMultiplier = multiplier.GetDownUp().GetValue();
        numberOfDownUp = multiplier.GetDownUp().GetUsesLeft();
        UpdateIdleMultipliers();
    }

    void Update()
    {
        upDownText.text = upDownMultiplier.ToString() + "x";
        horizontalText.text = horizontalMultiplier.ToString() + "x";
        downUpText.text = downUpMultiplier.ToString() + "x";

        UpdateFontSize(upDownText, upDownMultiplier);
        UpdateFontSize(horizontalText, horizontalMultiplier);
        UpdateFontSize(downUpText, downUpMultiplier);
        UpdateIdleMultipliers();
    }

    private void UpdateFontSize(TextMeshProUGUI text, int multiplier)
    {
        int baseSize = 50;
        int fontSize = baseSize;

        if (multiplier >= 100) fontSize += 10;
        if (multiplier >= 1000) fontSize += 10;

        text.fontSize = fontSize;
    }

    public void UpdateIdleMultipliers()
    {
        HalveIfZero(ref numberOfUpDown, ref upDownMultiplier);
        HalveIfZero(ref numberOfHorizontal, ref horizontalMultiplier);
        HalveIfZero(ref numberOfDownUp, ref downUpMultiplier);

        SetUpMinimumMultiplier();
        fileManager.SetMultiplierByWaifu(new MultiplierData(horizontalMultiplier, numberOfHorizontal), 
            new MultiplierData(upDownMultiplier, numberOfUpDown), new MultiplierData(downUpMultiplier, numberOfDownUp), fileManager.GetActiveWaifuName());
        SetRemainingUsage();
    }

    private void HalveIfZero(ref int count, ref int multiplier)
    {
        if (count == 0) {
            multiplier /= 2;
            if (multiplier > 2000) multiplier /= 2;
        }
    }

    private void SetUpMinimumMultiplier()
    {
        if (upDownMultiplier <= 1) {
            upDownMultiplier = 2;
        }
        
        if (horizontalMultiplier <= 0) {
            horizontalMultiplier = 1;
        }

        if (downUpMultiplier <= 1) {
            downUpMultiplier = 2;
        }
    }

    private void SetRemainingUsage()
    {
        upDownUsage.text = numberOfUpDown.ToString();
        horizontalUsage.text = numberOfHorizontal.ToString();
        downUpUsage.text = numberOfDownUp.ToString();
    }
}
