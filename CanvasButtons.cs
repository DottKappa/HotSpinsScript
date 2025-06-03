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
        if (!sceneManager.GetAutospinEnabled())
        {
            if (!sceneManager.MatrixHasEmptySlot() || !sceneManager.GetIsRolling())
            {
                if (sceneManager.GetIsRolling())
                {
                    sceneManager.StopSlot();
                }
                else
                {
                    sceneManager.StartSlot();
                }
            }
        }
    }

    public void StopLeftColumn()
    {
        if (!sceneManager.IsBusy()) sceneManager.StopSlotByColumn(SlotColumns.First);
    }

    public void StopCenterColumn()
    {
        if (!sceneManager.IsBusy()) sceneManager.StopSlotByColumn(SlotColumns.Second);
    }
    
    public void StopRightColumn()
    {
        if (!sceneManager.IsBusy()) sceneManager.StopSlotByColumn(SlotColumns.Third);
    }

    public void PickUpSpark()
    {
        sceneManager.PickUpSpark();
    }
}
