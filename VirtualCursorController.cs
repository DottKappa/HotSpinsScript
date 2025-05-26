using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class VirtualCursorController : MonoBehaviour
{
    public RectTransform cursorRect;
    public float speed = 2000f;

    private bool controllerConnected = false;
    private bool inputEnabled = false;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.UI.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.UI.Move.canceled += ctx => moveInput = Vector2.zero;
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
        if (!inputEnabled) return;

        // Muovi il cursore nel canvas
        Vector3 pos = cursorRect.localPosition;
        pos += new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime;

        RectTransform canvasRect = cursorRect.parent as RectTransform;
        Vector2 canvasSize = canvasRect.sizeDelta;

        pos.x = Mathf.Clamp(pos.x, -canvasSize.x / 2, canvasSize.x / 2);
        pos.y = Mathf.Clamp(pos.y, -canvasSize.y / 2, canvasSize.y / 2);

        cursorRect.localPosition = pos;

        // Gestione click
        if (controls.UI.Submit.triggered)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorRect.position);
            pointerData.position = screenPos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                Button btn = result.gameObject.GetComponent<Button>();
                if (btn != null && btn.IsActive() && btn.interactable)
                {
                    btn.onClick.Invoke();
                    break;
                }
            }
        }
    }

    public void SetControllerConnected(bool connected)
    {
        controllerConnected = connected;
        inputEnabled = connected;
        cursorRect.gameObject.SetActive(connected);
    }
}
