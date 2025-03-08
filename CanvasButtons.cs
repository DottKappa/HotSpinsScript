using UnityEngine;

public class CanvasButtons : MonoBehaviour
{
    public void StopSpins()
    {
        SlotController[] slotControllers = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        foreach (SlotController slotController in slotControllers) {
            slotController.SetMoving(false);
        }

// TODO -> eliminare il log della matrice
        SceneManager sceneManager = FindFirstObjectByType<SceneManager>();
        sceneManager.LogMatrix();
        sceneManager.RoundPositionByMatrix();
    }
}
