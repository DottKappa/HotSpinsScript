using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CollectionPage : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar horizontalScrollbar;
    private float scrollSpeed = 1f;
    private CollectionWaifu[] childScripts;
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    private FileManager fileManager;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.Cancel.performed += ctx => ReturnButton();
    }

    void Start()
    {
        childScripts = GetComponentsInChildren<CollectionWaifu>();
        SetAllChildInactive();
        GameObject fileManagerObject = GameObject.Find("FileManager");
        if (fileManagerObject != null) {
            fileManager = fileManagerObject.GetComponent<FileManager>();
        } else {
            Debug.LogError("[CollectionPage.cs] Non ho trovato l'elemento FileManager");
        }
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
        // Controlla lo scorrimento della rotella del mouse
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0) {
            ScrollWithMouse(scrollInput);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    public void SetAllChildInactive()
    {
        string activeWaifu = PlayerPrefs.GetString("waifuName");
        foreach (CollectionWaifu script in childScripts) {
            if (script.name != activeWaifu) {
                script.SetWaifuInactive();
            }
        }
    }

    public void ReturnButton()
    {
        fileManager.SaveWaifuFile();
        collectionPageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);        
    }

    private void ScrollWithMouse(float scrollInput)
    {
        // Modifica la posizione orizzontale in base allo scroll
        float newHorizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition - scrollInput * scrollSpeed;

        // Limita il valore per evitare che esca dai limiti
        newHorizontalNormalizedPosition = Mathf.Clamp01(newHorizontalNormalizedPosition);

        // Imposta la posizione orizzontale senza farla "resettare"
        scrollRect.horizontalNormalizedPosition = newHorizontalNormalizedPosition;
    }
}
