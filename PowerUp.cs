using UnityEngine;
using System.Collections;
using TMPro;

public class PowerUp : MonoBehaviour
{
    public Vector3 startPositionFirstCard = new Vector3(12, 0, 75);
    public Vector3 endPositionFirstCard = new Vector3(0, 0, 80);
    public Vector3 startPositionSecondCard = new Vector3(18, 0, 75);
    public Vector3 endPositionSecondCard = new Vector3(6.5f, 0, 80);
    public Quaternion endRotation = Quaternion.Euler(90, 0, -180);
    public float duration = 1f; // Durata della transizione in secondi
    public bool isFirstCard = true;
    private string keyToPickUp;
    private string whoAmI;
    private CanvasController canvasController;
    private PowerUpManager powerUpManager;

    void Start()
    {
        if (isFirstCard) {
            transform.position = startPositionFirstCard;
        } else {
            transform.position = startPositionSecondCard;
        }

        StartCoroutine(MoveToPosition());
        canvasController = FindFirstObjectByType<CanvasController>();
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform == transform) {
                    Debug.Log("Mouse button was pressed on object with tag: " + hit.transform.tag);
                    Debug.Log("Mouse button was pressed on object: " + whoAmI);
                    powerUpManager.ManagePowerUp(whoAmI);
                    canvasController.ToggleCanvasElements(true);
                }
            }
        } else if (Input.GetKeyDown(keyToPickUp)) {
            Debug.Log("Ho cliccato il tasto -> " + keyToPickUp);
            powerUpManager.ManagePowerUp(whoAmI);
            canvasController.ToggleCanvasElements(true);
        }
    }

    IEnumerator MoveToPosition()
    {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if (isFirstCard) {
                transform.position = Vector3.Lerp(startPositionFirstCard, endPositionFirstCard, t);
            } else {
                transform.position = Vector3.Lerp(startPositionSecondCard, endPositionSecondCard, t);
            }
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // Ensure the final position and rotation are set
        if (isFirstCard) {
            transform.position = endPositionFirstCard;
        } else {
            transform.position = endPositionSecondCard;
        }
        transform.rotation = endRotation;
    }

    public void SetKeyToPickUp(string key)
    {
        keyToPickUp = key;
    }

    public void SetText(string title, string text)
    {
        TextMeshPro[] textMeshes = GetComponentsInChildren<TextMeshPro>();
        if (textMeshes.Length >= 2) {
            textMeshes[0].text = title;
            textMeshes[1].text = text;
        }
    }

    public void SetWhoAmI(string enumString)
    {
        whoAmI = enumString;
    }
}
