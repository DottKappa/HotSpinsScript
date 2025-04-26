using UnityEngine;

public class PointSystemIdleController : MonoBehaviour
{
    private int numberOfHorizontal = 0;
    private int numberOfUpDown = 0;
    private int numberOfDownUp = 0;
    private int horizontalMultiplier = 1;
    private int upDownMultiplier = 1;
    private int downUpMultiplier = 1;

// == SET
    public void SetNumberOfHorizontal(int value) { numberOfHorizontal += value; }
    public void SetNumberOfUpDown(int value) { numberOfUpDown += value; }
    public void SetNumberOfDownUp(int value) { numberOfDownUp += value; }
    public void SetHorizontalMultiplier(int multiplier) { horizontalMultiplier = horizontalMultiplier * multiplier; }
    public void SetUpDownlMultiplier(int multiplier) { upDownMultiplier = upDownMultiplier * multiplier; }
    public void SetDownUpMultiplier(int multiplier) { downUpMultiplier = downUpMultiplier * multiplier; }

// == GET
    public int GetNumberOfHorizontal() { return numberOfHorizontal; }
    public int GetNumberOfUpDown() { return numberOfUpDown; }
    public int GetNumberOfDownUp() { return numberOfDownUp; }
    public int GetHorizontalMultiplier() { return horizontalMultiplier; }
    public int GetUpDownlMultiplier() { return upDownMultiplier; }
    public int GetDownUpMultiplier() { return downUpMultiplier; }
}
