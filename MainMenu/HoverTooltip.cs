using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup hoverCanvasGroup;  // CanvasGroup invece di GameObject
    public float fadeDuration = 0.2f;

    Coroutine fadeCoroutine;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(1f); // Fade in
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        StartFade(0f); // Fade out
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(targetAlpha));
    }

    IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        float startAlpha = hoverCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            hoverCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        hoverCanvasGroup.alpha = targetAlpha;
    }
}