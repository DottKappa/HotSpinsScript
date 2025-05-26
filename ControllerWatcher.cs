using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerWatcher : MonoBehaviour
{
    public VirtualCursorController cursorController;

    void Awake()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        UpdateControllerConnection();
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            UpdateControllerConnection();
        }
    }

    private void UpdateControllerConnection()
    {
        bool isConnected = Gamepad.all.Count > 0;

        if (cursorController != null)
        {
            cursorController.SetControllerConnected(isConnected);
        }
    }
}
