using UnityEngine;

public class CanvasButtons : MonoBehaviour
{
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
    }

    public void StartStopSpins()
    {
        if (!sceneManager.MatrixHasEmptySlot() || !sceneManager.GetIsRolling()) {
            if (sceneManager.GetIsRolling()) {
                sceneManager.StopSlot();
            } else {
                sceneManager.StartSlot();
            }
        }
    }

    public void StopLeftColumn()
    {
        sceneManager.StopSlotByColumn(SlotColumns.First);
    }

    public void StopCenterColumn()
    {
        sceneManager.StopSlotByColumn(SlotColumns.Second);
    }
    
    public void StopRightColumn()
    {
        sceneManager.StopSlotByColumn(SlotColumns.Third);
    }
}
