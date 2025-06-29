using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class AutospinManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup canvasGroup;             // Assegna dall'Inspector
    public float fadeDuration = 1f;
    public Image autospinButtonImage;

    private bool unlocked = true;               // Se true, effetto attivo
    private Coroutine fadeCoroutine;

    private FileManager fileManager;
    private SceneManager sceneManager;
    private bool isCoroutineRunning = false;
    private Coroutine autospinCoroutine;

    void Start()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        sceneManager = FindFirstObjectByType<SceneManager>();
        // Avvia il controllo ogni 3 secondi per vedere se è stato sbloccato
        StartCoroutine(CheckAutoSpinUnlockCoroutine());
        // Controlla subito lo stato iniziale
        if (PlayerPrefs.GetInt("autospinUnlocked", 1) == 0)
        {
            unlocked = false;
            canvasGroup.alpha = 0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!unlocked || canvasGroup == null)
            return;

        // Avvia solo il fade in
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f, 1f, fadeDuration));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!unlocked || canvasGroup == null)
            return;

        // Avvia solo il fade out
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup.alpha, 0f, fadeDuration));
    }

    IEnumerator FadeCanvasGroup(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    IEnumerator CheckAutoSpinUnlockCoroutine()
    {
        while (unlocked)
        {
            yield return new WaitForSeconds(3f);

            int total = 0;
            foreach (Waifu waifu in System.Enum.GetValues(typeof(Waifu)))
            {
                total += fileManager.GetSpinsByWaifu(waifu);
            }

            if (total >= 1999 && PlayerPrefs.GetInt("autospinUnlocked", 1) == 1)
            {
                PlayerPrefs.SetInt("autospinUnlocked", 0);
                PlayerPrefs.Save();

                unlocked = false;

                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                if (canvasGroup != null)
                    canvasGroup.alpha = 0f;

                break; // esci dalla coroutine: non serve più controllare
            }
        }
    }

    public void EnableAutospin()
    {
        if (PlayerPrefs.GetInt("autospinUnlocked", 1) == 0)
        {
            if (!sceneManager.GetAutospinEnabled())
            {
                sceneManager.SetAutospinEnabled(true);
                autospinButtonImage.color = new Color32(0x00, 0xD9, 0x0C, 0xFF);

                if (!isCoroutineRunning)
                {
                    autospinCoroutine = StartCoroutine(AutospinRoutine());
                }
            }
            else
            {
                sceneManager.SetAutospinEnabled(false);
                autospinButtonImage.color = new Color32(0x00, 0x00, 0x00, 0xFF);
                // Il loop si interromperà alla fine del ciclo corrente
            }
        }
    }

    private IEnumerator AutospinRoutine()
    {
        isCoroutineRunning = true;

        while (sceneManager.GetAutospinEnabled())
        {
            sceneManager.StartSlot();
            yield return new WaitForSeconds(1.5f); // Tempo di spin

            while (sceneManager.IsBusy())
            {
                yield return null; // Aspetta un frame e riprova
            }
            
            sceneManager.StopSlot();
            yield return new WaitForSeconds(1f); // Pausa prima del prossimo spin
        }

        isCoroutineRunning = false;
    }
}