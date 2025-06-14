using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CollectionPage : MonoBehaviour
{
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
}
