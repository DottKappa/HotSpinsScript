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

    void Awake()
    {
        pointSystemController = FindFirstObjectByType<PointSystemController>();
    }

    void Start() {
        sceneManager = FindFirstObjectByType<SceneManager>();
        fileManager = FindFirstObjectByType<FileManager>();
        FindSlotObj();
        SetNextArtAt();
    }

    void Update() {
        numberOfSlotsText.text = "Spins: " + addDot(sceneManager.GetNumberOfSpins());
        if (Input.GetKeyDown(KeyCode.C)) {
            string waifuName = fileManager.GetActiveWaifuName().ToString();
            if (waifuHidden == false) {
                SetWaifuImage(waifuName, "HideWaifu");
                waifuHidden = true;
            } else {
                waifuHidden = false;
                SetWaifuImage(waifuName, waifuName+"_"+pointSystemController.GetActualImageStep().ToString());
            }            
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

    private void FindSlotObj()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects) {
            if (!obj.activeInHierarchy) continue;

            string tag = obj.tag;
            if (tag == "Slot" || tag.Contains("_SlotCell")) {
                slotObjects.Add(obj.transform);
                originalSlotObjectsPositions.Add(obj.transform.position);
            }
        }
    }

    private void SetNextArtAt()
    {
        int pointsForNextArt = pointSystemController.GetPointsForNextArt();
        
        if (pointsForNextArt != -999) {
            nextArtAt.text = "Next art at: " + addDot(pointsForNextArt);
        } else {
            // Vuol dire che ho finito le art
            nextArtAt.text = "All arts unlocked";
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
}
