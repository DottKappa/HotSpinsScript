using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBumpScaler : MonoBehaviour
{
    public Image targetImage;
    public float scaleUp = 1.2f;
    public float duration = 0.2f;
    public float returnDuration = 0.1f;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    private ProgressBarTimer progressBarTimer;
    private bool isAnimating = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    void Start()
    {
        Transform progressBarTimerTransform = transform.parent.parent.Find("ProgressObj");
        progressBarTimer = progressBarTimerTransform.GetComponent<ProgressBarTimer>();
    }

    public void PopUpGiftButton()
    {
        if (isAnimating) return;

        StartCoroutine(PlayShowAnimation());
    }

    IEnumerator PlayShowAnimation()
    {
        isAnimating = true;

        // Mostra canvas (alpha 1)
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScale = originalScale * scaleUp;

        float t = 0f;
        while (t < duration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = targetScale;

        t = 0f;
        while (t < returnDuration)
        {
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t / returnDuration);
            t += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = originalScale;

        isAnimating = false;
    }

    public void GiftButton()
    {
        Debug.Log("Ecco il regalo");
        progressBarTimer.PickUpGift();
    }

    public void HideButton()
    {
        if (canvasGroup != null) {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
