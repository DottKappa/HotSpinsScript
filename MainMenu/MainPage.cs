using UnityEngine;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;

public class MainPage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    public Canvas rulesPageCanvas;
    public Canvas optionPageCanvas;
    public GameObject fullScreenImage;
    
    public void PlayButton()
    {
        Debug.Log("Hai premuto play");

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
        Debug.Log("Hai premuto collection");
        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(true);
        fullScreenImage.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OptionButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        optionPageCanvas.gameObject.SetActive(true);
        Debug.Log("Hai premuto option");
    }

    public void ExitButton()
    {
        Debug.Log("Hai premuto exit");
        Application.Quit();
    }

    public void RulesButton()
    {
        mainPageCanvas.gameObject.SetActive(false);
        rulesPageCanvas.gameObject.SetActive(true);
        Debug.Log("Hai premuto rules");
    }
}
