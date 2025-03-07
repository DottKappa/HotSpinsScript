using UnityEngine;

public class CanvasButtons : MonoBehaviour
{
    public void StopSpins()
    {
        SlotController[] slotControllers = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        foreach (SlotController slotController in slotControllers) {
            slotController.SetMoving(false);
        }
    }
}
