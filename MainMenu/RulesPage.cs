using UnityEngine;

public class RulesPage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas rulesPageCanvas;
    
    public void ReturnButton()
    {
        rulesPageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);
    }
}
