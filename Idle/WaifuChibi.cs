using UnityEngine;
using System.Collections;

public class WaifuChibi : MonoBehaviour
{
    public float leftX = -1.4f;
    public float rightX = 2f;
    public float speed = 1f;

    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.03f;
    public float bounceFrequency = 4f;

    [Header("Flip Delay")]
    public float flipDelay = 0.4f;

    [Header("Idle Breath")]
    public float breathScaleAmount = 0.000005f;
    public float breathSpeed = 2f;

    private RectTransform rectTransform;
    private bool goingLeft = true;
    private bool isWaiting = false;
    private float baseY;
    private Vector3 originalScale;
    private bool canMove = true;
    private Vector3 initialScale;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(rightX, rectTransform.localPosition.y, rectTransform.localPosition.z);
        baseY = rectTransform.localPosition.y;

        initialScale = rectTransform.localScale;
        originalScale = initialScale;

        SetFacingDirection();
    }

    private void Update()
    {
        if (canMove) {
            if (rectTransform == null) return;

            Vector3 pos = rectTransform.localPosition;

            if (isWaiting)
            {
                IdleBreath(); // effetto idle mentre fermo
                return;
            }
            else
            {
                // Reset scala Y quando si muove
                Vector3 scale = rectTransform.localScale;
                scale.y = initialScale.y;
                rectTransform.localScale = scale;
            }

            float targetX = goingLeft ? leftX : rightX;
            pos.x = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime);
            pos.y = ApplyBounce();

            rectTransform.localPosition = pos;

            if (Mathf.Approximately(pos.x, targetX))
            {
                StartCoroutine(FlipAfterDelay());
            }
        }
    }

    public void StartStopWaifu(bool can = false)
    {
        canMove = can;
    }

    private IEnumerator FlipAfterDelay()
    {
        isWaiting = true;

        yield return new WaitForSeconds(flipDelay);

        goingLeft = !goingLeft;
        SetFacingDirection();

        // NON resettare l'intera scala, altrimenti perdi il flip
        // Reset solo la scala Y
        Vector3 scale = rectTransform.localScale;
        scale.x = goingLeft ? Mathf.Abs(initialScale.x) : -Mathf.Abs(initialScale.x);
        rectTransform.localScale = scale;

        isWaiting = false;
    }

    private void SetFacingDirection()
    {
        Vector3 scale = rectTransform.localScale;
        float direction = goingLeft ? 1f : -1f;
        scale.x = direction * Mathf.Abs(originalScale.x);
        rectTransform.localScale = scale;
    }

    private float ApplyBounce()
    {
        return baseY + Mathf.Sin(Time.time * bounceFrequency) * bounceAmplitude;
    }

    private void IdleBreath()
    {
        // Oscillazione scala Y senza toccare X (flip)
        float breath = Mathf.Sin(100 * breathSpeed) * breathScaleAmount;

        Vector3 newScale = initialScale;
        newScale.y += breath;
        newScale.x = rectTransform.localScale.x;
        rectTransform.localScale = newScale;
    }
}