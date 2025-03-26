using UnityEngine;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;

public class MainPage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    
    public void PlayButton()
    {
        Debug.Log("Hai premuto play");

        string waifuName = PlayerPrefs.GetString("waifuName");
        if (string.IsNullOrEmpty(waifuName)) {
            waifuName = Waifu.Chiho.ToString();
            PlayerPrefs.SetString("waifuName", waifuName);
            PlayerPrefs.Save();
        }

        SceneManagement.LoadScene("Slot");
    }

    public void CollectionButton()
    {
        Debug.Log("Hai premuto collection");
        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(true);
    }

    public void OptionButton()
    {
        Debug.Log("Hai premuto option");
    }

    public void ExitButton()
    {
        Debug.Log("Hai premuto exit");
        Application.Quit();
    }

    public void RulesButton()
    {
        Debug.Log("Hai premuto rules");
    }
}
