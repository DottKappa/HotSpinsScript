using UnityEngine;
using UnityEngine.UI;

public class CollectionPage : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar horizontalScrollbar;
    private float scrollSpeed = 1f;
    private CollectionWaifu[] childScripts;
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;

    void Start()
    {
        childScripts = GetComponentsInChildren<CollectionWaifu>();
        SetAllChildInactive();
    }

    void Update()
    {
        // Controlla lo scorrimento della rotella del mouse
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0) {
            ScrollWithMouse(scrollInput);
        }
    }

    public void SetAllChildInactive()
    {
        foreach (CollectionWaifu script in childScripts) {
            script.SetWaifuInactive();
        }
    }

    public void ReturnButton()
    {
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
