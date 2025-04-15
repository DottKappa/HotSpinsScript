using UnityEngine;
using System.Collections;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;

public class TutorialPage : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool loadScene = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetLoadScene(bool loadScene)
    {
        this.loadScene = loadScene;
    }

    public void CloseTutorialButton()
    {
        if (loadScene) {
            PlayerPrefs.SetInt("seeTutorial", 0);
            PlayerPrefs.Save();
            SceneManagement.LoadScene("Slot");
        }
        StartCoroutine(FadeOut(0.2f));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration) {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
