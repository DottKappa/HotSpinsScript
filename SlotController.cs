using UnityEngine;
using System.Collections;

public class SlotController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private bool movingUp = true;
    private Vector2 originalPosition;
    private bool moving = true;
    private SceneManager sceneManager;

    // Riferimenti per animazione
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        sceneManager = Object.FindFirstObjectByType<SceneManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x * 2, originalScale.y * 2, originalScale.z);
    }

    void Update()
    {
        if (moving) {
            MoveCell();
        } else {
            rb.linearVelocity = new Vector2(0, 0);
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

    // Funzione per aumentare lo scaling gradualmente
    public void IncreaseScaleGradually()
    {
        StartCoroutine(ScaleCoroutine());
        if (tag.Contains("Special_")) {
            StartCoroutine(RotateY());
        }
    }

    // Coroutine che cambia la scala gradualmente
    private IEnumerator ScaleCoroutine()
    {
        float timeElapsed = 0f; // Tempo trascorso
        float duration = 0.3f; // Durata dell'animazione
        
        // Interpoliamo gradualmente dalla scala originale a quella ingrandita
        while (timeElapsed < duration) {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime; // Aumentiamo il tempo trascorso
            yield return null; // Aspettiamo il prossimo frame
        }

        // Assicuriamoci che alla fine la scala arrivi esattamente al target
        transform.localScale = targetScale;

        // Interpoliamo di nuovo indietro verso la scala originale
        timeElapsed = 0f;
        while (timeElapsed < duration) {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Assicuriamoci che torni esattamente alla scala originale
        transform.localScale = originalScale;
    }

    private IEnumerator RotateY()
    {
        float rotationDuration = 0.3f;
        float elapsedTime = 0f;
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360f;

        while (elapsedTime < rotationDuration) {
            float yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Rotazione finale precisa
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);
    }
}
