using UnityEngine;

public class RulesIdlePage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas rulesIdlePageCanvas;
    
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
