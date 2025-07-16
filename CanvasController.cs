using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI numberOfSlotsText;
    public TextMeshProUGUI nextArtAt;
    private SceneManager sceneManager;
    private PointSystemController pointSystemController;
    private FileManager fileManager;
    private List<Transform> slotObjects = new List<Transform>();
    private List<Vector3> originalSlotObjectsPositions = new List<Vector3>();
    private bool waifuHidden = false;
    private bool hasMultiplierBuff = false;
    public void SetHasMultiplierBuff(bool value) => hasMultiplierBuff = value;
    private bool hasDivideDebuff = false;
    public void SetHasDivideDebuff(bool value) => hasDivideDebuff = value;
    private InputSystem_Actions controls;
    private Sprite previousWaifuSprite;
    private int lastSpin = -1;

    void Awake()
    {
        pointSystemController = FindFirstObjectByType<PointSystemController>();
        controls = new InputSystem_Actions();
        controls.UI.Hide.performed += ctx => HideWaifu();
    }

    void OnEnable() { controls.Enable(); }
    void OnDisable() { controls.Disable(); }

    void Start() {
        sceneManager = FindFirstObjectByType<SceneManager>();
        fileManager = FindFirstObjectByType<FileManager>();
        FindSlotObj();
        SetNextArtAt();
    }

    void Update() {
        int currentSpin = sceneManager.GetNumberOfSpins();
        if (currentSpin != lastSpin)
        {
            lastSpin = currentSpin;
            SetTextOfSpins(currentSpin);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            HideWaifu();
        }
    }

    private void HideWaifu()
    {
        string waifuName = fileManager.GetActiveWaifuName().ToString();
        if (waifuHidden == false) {
            SetWaifuImage(waifuName, "HideWaifu");
            waifuHidden = true;
        } else {
            waifuHidden = false;
            RestorePreviousWaifuImage();
        }
    }

    private void SetTextOfSpins(int numberOfSpin)
    {
        numberOfSlotsText.text = addDot(numberOfSpin);
        if (numberOfSpin % 5 == 0 && hasMultiplierBuff)
        {
            numberOfSlotsText.color = new Color32(0x23, 0xCC, 0x15, 0xFF);
        }
        else if (numberOfSpin % 11 == 0 && hasDivideDebuff)
        {
            numberOfSlotsText.color = new Color32(0xCC, 0x1E, 0x15, 0xFF);
        }
        else
        {
            numberOfSlotsText.color = Color.black;
        }
    }

    public void ToggleCanvasElements(bool isActive)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
            if (child.name == "CanvasSlotContainer") {
                child.gameObject.SetActive(isActive);
            }
        }

        if (isActive) {
            GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerUp in powerUps) {
                Destroy(powerUp);
            }
        }
    }

    private string addDot(int points)
    {
        string pointsString = points.ToString();
        int length = pointsString.Length;

        if (length <= 3) return pointsString;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int counter = 0;

        for (int i = length - 1; i >= 0; i--) {
            sb.Insert(0, pointsString[i]);
            counter++;
            if (counter == 3 && i != 0) {
                sb.Insert(0, '.');
                counter = 0;
            }
        }

        return sb.ToString();
    }

    public void SetWaifuImage(string imageFolder, string imageName)
    {
        SetNextArtAt();
        if (waifuHidden == false) {
            Transform waifuTransform = transform.Find("CanvasBackgroundContainer/Waifu");
            if (waifuTransform != null) {
                GameObject waifuImageObject = waifuTransform.gameObject;
                UnityEngine.UI.Image imageComponent = waifuImageObject.GetComponent<UnityEngine.UI.Image>();
                if (imageComponent != null) {
                    previousWaifuSprite = imageComponent.sprite;
                    Sprite newSprite = Resources.Load<Sprite>("Texture/Waifu/" + imageFolder + "/" + imageName);
                    if (newSprite != null) {
                        imageComponent.sprite = newSprite;
                    }
                    else {
                        Debug.LogError("Image not found: " + imageName);
                    }
                } else {
                    Debug.LogError("ImageComponent component not found on Waifu object.");
                }
            } else {
                Debug.LogError("Waifu object not found.");
            }
        }
    }
    
    private void RestorePreviousWaifuImage()
    {
        Transform waifuTransform = transform.Find("CanvasBackgroundContainer/Waifu");
        if (waifuTransform != null && previousWaifuSprite != null)
        {
            UnityEngine.UI.Image imageComponent = waifuTransform.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = previousWaifuSprite;
            }
        }
    }

    private void FindSlotObj()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy) continue;

            string tag = obj.tag;
            if (tag == "Slot" || tag.Contains("_SlotCell"))
            {
                slotObjects.Add(obj.transform);
                originalSlotObjectsPositions.Add(obj.transform.position);
            }
        }
    }

    private void SetNextArtAt()
    {
        int pointsForNextArt = pointSystemController.GetPointsForNextArt();
        string currentLanguage = null;
        currentLanguage = PlayerPrefs.GetString("language", currentLanguage);
        
        if (pointsForNextArt != -999) {
            switch (currentLanguage)
            {
                case "it":  nextArtAt.text = "Prossima im a: " + addDot(pointsForNextArt); break;
                case "fr":  nextArtAt.text = "Próximo arte en: " + addDot(pointsForNextArt); break;
                case "es":  nextArtAt.text = "Art suivant à: " + addDot(pointsForNextArt); break;
                case "en":
                default:    nextArtAt.text = "Next art at: " + addDot(pointsForNextArt); break;
            }            
        } else {
            // Vuol dire che ho finito le art
            switch (currentLanguage)
            {
                case "it":  nextArtAt.text = "Tutte le imm sbloc"; break;
                case "fr":  nextArtAt.text = "Arts débloqués"; break;
                case "es":  nextArtAt.text = "Artes desbloqueados"; break;
                case "en":
                default:    nextArtAt.text = "All arts unlocked"; break;
            }
        }
    }

    public void ShakeSlot()
    {
        for (int i = 0; i < slotObjects.Count; i++) {
            Transform obj = slotObjects[i];
            Vector3 originalPos = originalSlotObjectsPositions[i];
            obj.position = originalPos;
            StartCoroutine(ShakeObject(obj, originalPos));
        }
    }

    private IEnumerator ShakeObject(Transform obj, Vector3 originalPos)
    {
        float shakeDuration = 0.3f;
        float shakeIntensity = 0.1f;
        float elapsed = 0f;

        while (elapsed < shakeDuration) {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            obj.position = new Vector3(originalPos.x + offsetX, originalPos.y + offsetY, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = originalPos;
    }

    public void CheckIfWaifuIsHidden()
    {
        int points = pointSystemController.GetPoints();
        if (!waifuHidden) {
            if (points <= 10000) {
                pointSystemController.DividePoints(1.01f);
            } else if (points <= 30000) {
                pointSystemController.DividePoints(1.1f);
            } else if (points <= 50000) {
                pointSystemController.DividePoints(1.2f);
            } else {
                pointSystemController.DividePoints(1.5f);
            }
        } else {
            pointSystemController.MultipliePoints(1.01f);
        }
    }
}
