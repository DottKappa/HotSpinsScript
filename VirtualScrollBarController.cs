using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class VirtualScrollBarController : MonoBehaviour
{ 
    private List<ScrollRect> scrollRects = new List<ScrollRect>();
    public float scrollSpeed = 1f;
    private InputSystem_Actions controls;

    private int frameCounter = 0;
    private int framesBetweenRefresh = 60; // aggiorna ogni 60 frame

    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        frameCounter++;
        if (frameCounter >= framesBetweenRefresh)
        {
            frameCounter = 0;
            RefreshScrollRects();
        }

        float horizontalInput = controls.UI.ScrollHorizontal.ReadValue<float>();

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            foreach (var scrollRect in scrollRects)
            {
                scrollRect.horizontalNormalizedPosition += horizontalInput * scrollSpeed * Time.deltaTime;
                scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition);
            }
        }
    }

    void RefreshScrollRects()
    {
        scrollRects.Clear();
        scrollRects.AddRange(Object.FindObjectsByType<ScrollRect>(FindObjectsSortMode.None));
    }
}
