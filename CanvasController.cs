using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public TextMeshProUGUI numberOfSlotsText;
    private SceneManager sceneManager;

    void Start() {
        sceneManager = FindFirstObjectByType<SceneManager>();
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
}
