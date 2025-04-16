using UnityEngine;
using System.Collections;

public class RulesPage : MonoBehaviour
{
    public Canvas mainPageCanvas;
    public Canvas rulesPageCanvas;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ReturnButton();
        }
    }

    public void ReturnButton()
    {
        rulesPageCanvas.gameObject.SetActive(false);
        mainPageCanvas.gameObject.SetActive(true);
    }

    public void TutorialButton(bool loadScene = false)
    {
        GameObject targetObject = GameObject.Find("TutorialPage");
        TutorialPage tutorialPage = targetObject.GetComponent<TutorialPage>();
        tutorialPage.SetLoadScene(loadScene);
        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(FadeIn(canvasGroup, 0.5f));
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
