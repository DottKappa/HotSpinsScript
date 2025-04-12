using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI numberOfSlotsText;
    private SceneManager sceneManager;
    private PointSystemController pointSystemController;
    private FileManager fileManager;
    private List<Transform> slotObjects = new List<Transform>();
    private bool waifuHidden = false;

    void Start() {
        sceneManager = FindFirstObjectByType<SceneManager>();
        pointSystemController = FindFirstObjectByType<PointSystemController>();
        fileManager = FindFirstObjectByType<FileManager>();
        FindSlotObj();
    }

    void Update() {
        numberOfSlotsText.text = "Spins: " + addDot(sceneManager.GetNumberOfSpins());
        if (Input.GetKeyDown(KeyCode.C)) {
            string waifuName = fileManager.GetActiveWaifuName().ToString();
            if (waifuHidden == false) {
                SetWaifuImage(waifuName, "HideWaifu");
                waifuHidden = true;
            } else {
                SetWaifuImage(waifuName, waifuName+"_"+pointSystemController.GetActualImageStep().ToString());
                waifuHidden = false;
            }            
        }
    }

    public void ToggleCanvasElements(bool isActive)
    {
        foreach (Transform child in transform) {
            if (!child.CompareTag("Waifu") && !child.CompareTag("Background")) {
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
        if (waifuHidden == false) {
            Transform waifuTransform = transform.Find("Waifu");
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
            }
        }
    }

    public void ShakeSlot()
    {
        foreach (Transform obj in slotObjects) {
            StartCoroutine(ShakeObject(obj));
        }
    }

    private IEnumerator ShakeObject(Transform obj)
    {
        float shakeDuration = 0.3f;
        float shakeIntensity = 0.1f;
        Vector3 originalPos = obj.position;
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
