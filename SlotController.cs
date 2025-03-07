using UnityEngine;

public class SlotController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private bool movingUp = true;
    private Vector2 originalPosition;
    private bool moving = true;
    private SceneManager sceneManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        sceneManager = Object.FindFirstObjectByType<SceneManager>();
    }

    void Update()
    {
        if (moving) {
            MoveCell();
        } else {
            rb.linearVelocity = new Vector2(0, 0);
            RoundPosition();
        }
    }

    void MoveCell()
    {
        if (movingUp) {
            rb.linearVelocity = new Vector2(0, speed);
            if (transform.position.y >= originalPosition.y + 0.3f) {
                movingUp = false;
                speed = 15.0f;
            }
        }
        else if (transform.position.y >= 0.5f) {
            rb.linearVelocity = new Vector2(0, -speed);
        }
        
        if (transform.position.y <= 2.5f) {
            transform.Rotate(20 * Time.deltaTime, 0, 0);
        }
    }

    void RoundPosition()
    {
        Vector3[] startingPositions = sceneManager.GetAllStartingPosition();
            // Implementa la logica per arrotondare la posizione utilizzando startingPositions
            // Ad esempio, puoi trovare la posizione più vicina e impostarla come nuova posizione
        Vector3 closestPosition = startingPositions[0];
        float closestDistance = Vector3.Distance(transform.position, closestPosition);

        foreach (Vector3 position in startingPositions)
        {
            float distance = Vector3.Distance(transform.position, position);
            if (distance < closestDistance)
            {
                closestPosition = position;
                closestDistance = distance;
            }
        }
        Debug.Log("Posizione y prima dell'arrotondamento: " + transform.position.y);
        transform.position = closestPosition;
        Debug.Log("Posizione y dopo l'arrotondamento: " + transform.position.y);
    }

    public void SetConstructorValues(bool movingUp, float speed, bool moving = true)
    {
        this.movingUp = movingUp;
        this.speed = speed;
        this.moving = moving;
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }
}
