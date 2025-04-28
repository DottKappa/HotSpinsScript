using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutosaveImage : MonoBehaviour
{
    public float rotationDuration = 1f;
    public float delayBetweenSpins = 0.5f;
    private CanvasGroup cg;

    void Start()
    {
        cg = gameObject.GetComponent<CanvasGroup>();
    }

    public void SaveAnimation()
    {
        cg.alpha = 1;
        StartCoroutine(SpinTwice());
    }

    IEnumerator SpinTwice()
    {
        yield return RotateZ(360, rotationDuration);
        yield return new WaitForSeconds(delayBetweenSpins);
        yield return RotateZ(360, rotationDuration);
        cg.alpha = 0;
    }

    IEnumerator RotateZ(float angle, float duration)
    {
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + angle;
        float t = 0;

        while (t < 1) {
            t += Time.deltaTime / duration;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t);
            transform.eulerAngles = new Vector3(0, 0, zRotation);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, 0, endRotation);
    }
}
