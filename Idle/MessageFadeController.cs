using System.Collections;
using UnityEngine;
using TMPro;

public class MessageFadeController : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float visibleDuration = 5f;

    private CanvasGroup canvasGroup;
    private TextMeshProUGUI tmpText;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) {
            Debug.LogError("[MessageFadeController] CanvasGroup mancante sul GameObject Message.");
        }

        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText == null) {
            Debug.LogError("[MessageFadeController] TextMeshProUGUI mancante tra i figli.");
        }

        canvasGroup.alpha = 0f; // Parte invisibile
    }

    public void ShowText(string message)
    {
        if (tmpText == null || canvasGroup == null) return;

        tmpText.text = message;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));         // Fade-in
        yield return new WaitForSeconds(visibleDuration);                // Pausa visibile
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));         // Fade-out
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
