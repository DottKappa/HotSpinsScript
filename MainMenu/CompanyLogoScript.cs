using UnityEngine;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;

public class CompanyLogoScript : MonoBehaviour
{
    public RectTransform targetUI;
    public float duration = 2.0f;

    private void Awake()
    {
        if (targetUI != null)
        {
            StartCoroutine(MoveAndScaleUI());
        }
    }

    private System.Collections.IEnumerator MoveAndScaleUI()
    {
        // Attesa di 2 secondi prima di iniziare l'animazione
        yield return new WaitForSeconds(0.5f);

        Vector2 startAnchoredPos = targetUI.anchoredPosition;
        Vector2 targetAnchoredPos = new Vector2(startAnchoredPos.x, 120f);

        Vector3 startScale = targetUI.localScale;
        Vector3 targetScale = new Vector3(1.1f, 1.1f, startScale.z);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            targetUI.anchoredPosition = Vector2.Lerp(startAnchoredPos, targetAnchoredPos, t);
            targetUI.localScale = Vector3.Lerp(startScale, targetScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Valori finali precisi
        targetUI.anchoredPosition = targetAnchoredPos;
        targetUI.localScale = targetScale;

        SceneManagement.LoadScene("Menu");
    }
}