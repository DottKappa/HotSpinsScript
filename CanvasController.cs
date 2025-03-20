using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI numberOfSlotsText;
    private SceneManager sceneManager;
    private PointSystemController pointSystemController;

    void Start() {
        sceneManager = FindFirstObjectByType<SceneManager>();
        pointSystemController = FindFirstObjectByType<PointSystemController>();
    }

    void Update() {
        numberOfSlotsText.text = "Spins: " + addDot(sceneManager.GetNumberOfSpins());
    }

    public void ToggleCanvasElements(bool isActive)
    {
        foreach (Transform child in transform) {
            if (!child.CompareTag("Waifu")) {
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
            }
            else
            {
                Debug.LogError("ImageComponent component not found on Waifu object.");
            }
        } else {
            Debug.LogError("Waifu object not found.");
        }
    }
}
