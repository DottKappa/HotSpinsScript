using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public Camera mainCamera;
    public Vector2 referenceResolution = new Vector2(2560, 1440);
    public float referenceOrthoSize = 5f;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        UpdateCameraSize();
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateCameraSize();
        }
    }

    private int lastScreenWidth = 0;
    private int lastScreenHeight = 0;

    void UpdateCameraSize()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        float referenceAspect = referenceResolution.x / referenceResolution.y;

        float sizeMultiplier = referenceAspect / currentAspect;
        mainCamera.orthographicSize = referenceOrthoSize * sizeMultiplier;

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
