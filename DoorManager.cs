using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorManager : MonoBehaviour
{
    public Image targetImage;
    public CanvasGroup canvasGroup;
    public AudioSource audioSource;
    public CanvasController canvasController;

    private Coroutine pulseCoroutine = null;
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager = FindFirstObjectByType<SceneManager>();
        StartCoroutine(CheckForPulse());
    }

    IEnumerator CheckForPulse()
    {
        float elapsedTime = 0f;
        float maxTime = 480f; // 8 minuti
        float minTime = 240f; // 4 minuto

        while (true) {
            if (gameObject.activeInHierarchy && !sceneManager.GetAutospinEnabled()) {
                elapsedTime += Time.deltaTime;

                float normalizedTime = Mathf.InverseLerp(minTime, maxTime, elapsedTime);
                float chance = normalizedTime;

                if (Random.value < chance) {
                    // Trigger Pulse + Audio
                    if (pulseCoroutine == null) {
                        pulseCoroutine = StartCoroutine(PulseEffectLoop());
                    }

                    elapsedTime = 0f; // reset timer
                    yield return new WaitUntil(() => !audioSource.isPlaying);

                    // Cleanup after audio ends
                    if (pulseCoroutine != null) {
                        StopCoroutine(pulseCoroutine);
                        pulseCoroutine = null;
                    }

                    canvasGroup.alpha = 0f; // Hide image

                    if (canvasController != null) {
                        canvasController.CheckIfWaifuIsHidden();
                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator PulseEffectLoop()
    {
        // Start audio once
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.Play();
        }

        float pulseDuration = 1f;

        while (audioSource != null && audioSource.isPlaying) {
            float elapsed = 0f;

            while (elapsed < pulseDuration) {
                float alpha = Mathf.Sin(elapsed / pulseDuration * Mathf.PI);
                canvasGroup.alpha = alpha;
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }
    }
}
