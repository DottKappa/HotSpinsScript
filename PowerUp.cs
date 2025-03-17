using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    // Start position x=12, y=0, z=75
    public Vector3 endPosition = new Vector3(0, 0, 80);
    public Quaternion endRotation = Quaternion.Euler(90, 0, -180);
    public float duration = 1f; // Durata della transizione in secondi
    private string keyToPickUp;
    private CanvasController canvasController;

    void Start()
    {
        StartCoroutine(MoveToPosition());
        canvasController = FindFirstObjectByType<CanvasController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log("Mouse button was pressed on object with tag: " + hit.transform.tag);
                canvasController.ToggleCanvasElements(true);
            }
        } else if (Input.GetKeyDown(keyToPickUp)) {
            Debug.Log("Ho cliccato il tasto -> " + keyToPickUp);
            canvasController.ToggleCanvasElements(true);
        }
    }

    IEnumerator MoveToPosition()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // Ensure the final position and rotation are set
        transform.position = endPosition;
        transform.rotation = endRotation;
    }

    public void SetKeyToPickUp(string key)
    {
        keyToPickUp = key;
    }
}
