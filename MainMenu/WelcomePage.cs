using UnityEngine;
using System;
using System.Collections;

public class WelcomePage : MonoBehaviour
{
    public Canvas welcomePageCanvas;
    public Canvas mainPageCanvas;
    public Canvas collectionPageCanvas;
    public Canvas rulesPageCanvas;
    public Canvas optionPageCanvas;
    public Canvas idleCanvas;
    public Canvas rulesIdlePageCanvas;
    public CanvasGroup bgSlot;

    [Header("Welcome page")]
    public GameObject warpPage;

    private void Awake()
    {
        cam = Camera.main;
        originalSize = cam.orthographicSize;
        originalPosition = cam.transform.position;


        mainPageCanvas.gameObject.SetActive(false);
        collectionPageCanvas.gameObject.SetActive(false);
        rulesPageCanvas.gameObject.SetActive(false);
        optionPageCanvas.gameObject.SetActive(false);
        rulesIdlePageCanvas.gameObject.SetActive(false);
        idleCanvas.gameObject.SetActive(false);
        StartCoroutine(FadeOut(bgSlot));

        float oldVolume = PlayerPrefs.GetFloat("audioVolume", 0f);
        if (oldVolume != 0f)
        {
            AudioListener.volume = oldVolume;
        }

        if (PlayerPrefs.GetInt("skipWelcomePage") == 1)
        {
            PlayerPrefs.SetInt("skipWelcomePage", 0);
            FakeStart();
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = (PlayerPrefs.GetInt("vSyncEnabled", 1) == 1) ? 1 : 0;
    }

    public void StartButton()
    {
        StartCoroutine(ZoomAndMoveCoroutine());
        mainPageCanvas.gameObject.SetActive(true);
        idleCanvas.gameObject.SetActive(true);
        welcomePageCanvas.gameObject.SetActive(false);
    }

    private void FakeStart()
    {
        mainPageCanvas.gameObject.SetActive(true);
        idleCanvas.gameObject.SetActive(true);
        welcomePageCanvas.gameObject.SetActive(false);
    }

    public void StartZoomButton()
    {
        StartCoroutine(ZoomAndMoveCoroutine());
    }

    public void WarpButton()
    {
        warpPage.SetActive(true);
        welcomePageCanvas.gameObject.SetActive(false);
    }

    public void CreditsButton()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < 5f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / 5f);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }


    private float zoomedSize = 0.65f;             // Quanto zoommare (valore più basso = più zoom)
    private float zoomSpeed = 2f;              // Velocità dello zoom
    private float zoomedY = 2.3f; 
    private Camera cam;
    private float originalSize;
    private Vector3 originalPosition;

    private System.Collections.IEnumerator ZoomAndMoveCoroutine()
    {
        Vector3 targetPosition = new Vector3(originalPosition.x, zoomedY, originalPosition.z);

        // Zoom in e sposta in alto
        yield return StartCoroutine(ChangeZoomAndPosition(cam.orthographicSize, zoomedSize, cam.transform.position, targetPosition));

        CanvasGroup cg = welcomePageCanvas.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        zoomSpeed = 6f;

        // Zoom out e ritorna alla posizione originale
        yield return StartCoroutine(ChangeZoomAndPosition(cam.orthographicSize, originalSize, cam.transform.position, originalPosition));

        mainPageCanvas.gameObject.SetActive(true);
        idleCanvas.gameObject.SetActive(true);

        welcomePageCanvas.gameObject.SetActive(false);
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        zoomSpeed = 2f;
    }

    private System.Collections.IEnumerator ChangeZoomAndPosition(float fromSize, float toSize, Vector3 fromPos, Vector3 toPos)
    {
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * zoomSpeed;
            cam.orthographicSize = Mathf.Lerp(fromSize, toSize, elapsed);
            cam.transform.position = Vector3.Lerp(fromPos, toPos, elapsed);
            yield return null;
        }

        cam.orthographicSize = toSize;
        cam.transform.position = toPos;
    }
}
