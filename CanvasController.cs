using UnityEngine;

public class CanvasController : MonoBehaviour
{
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
}
