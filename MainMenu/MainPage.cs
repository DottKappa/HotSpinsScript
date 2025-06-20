using UnityEngine;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;
using System.Collections;

public class MainPage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    public Canvas rulesPageCanvas;
    public Canvas optionPageCanvas;
    public Canvas idleCanvas;
    public Canvas rulesIdlePageCanvas;
    public GameObject fullScreenImage;
    private IdleFileManager idleFileManager;
    public Canvas welcomePageCanvas;
    
    void Start()
    {
        idleFileManager = FindFirstObjectByType<IdleFileManager>();
    }

    private void OnEnable() 
    {
        if (idleCanvas != null) {
            idleCanvas.gameObject.SetActive(true);
            CanvasGroup cg = idleCanvas.gameObject.GetComponent<CanvasGroup>();
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }

    private void OnDisable() 
    {
        if (idleCanvas != null) {
            CanvasGroup cg = idleCanvas.gameObject.GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void PlayButton()
    {
        idleFileManager.SaveIdleFile();

        string waifuName = PlayerPrefs.GetString("waifuName");
        if (string.IsNullOrEmpty(waifuName)) {
            waifuName = Waifu.Chiho.ToString();
            PlayerPrefs.SetString("waifuName", waifuName);
            PlayerPrefs.Save();
        }

        int seeTutorial = PlayerPrefs.GetInt("seeTutorial", 1);
        if (seeTutorial == 1) {
            rulesPageCanvas.gameObject.SetActive(true);
            foreach (Transform child in rulesPageCanvas.transform) {
                child.gameObject.SetActive(false);
            }
            RulesPage rulesScript = rulesPageCanvas.GetComponent<RulesPage>();
            rulesScript.TutorialButton(true);
        } else {
            SceneManagement.LoadScene("Slot");
        }
    }

    public void CollectionButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(true);
        fullScreenImage.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OptionButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        optionPageCanvas.gameObject.SetActive(true);
    }

    public void ExitButton()
    {
        idleFileManager.SaveIdleFile();
        Application.Quit();
    }

    public void ReturnButton()
    {
        idleFileManager.SaveIdleFile();
        mainPageCanvas.gameObject.SetActive(false);
        idleCanvas.gameObject.SetActive(false);
        welcomePageCanvas.gameObject.SetActive(true);
    }

    public void RulesButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        rulesPageCanvas.gameObject.SetActive(true);
    }

    public void RulesIdleButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        rulesIdlePageCanvas.gameObject.SetActive(true);
    }
}
