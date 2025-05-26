using UnityEngine;

public class RulesIdlePage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas rulesIdlePageCanvas;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.Cancel.performed += ctx => ReturnButton();
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    public void ReturnButton()
    {
        rulesIdlePageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);
    }
}
