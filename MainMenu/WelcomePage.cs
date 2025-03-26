using UnityEngine;

public class WelcomePage : MonoBehaviour
{
    public Canvas welcomePageCanvas;
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;

    private void Awake() {
        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(false);
    }

    public void StartButton()
    {
        mainPageCanvas.gameObject.SetActive(true);
        Destroy(welcomePageCanvas.gameObject);
    }
}
