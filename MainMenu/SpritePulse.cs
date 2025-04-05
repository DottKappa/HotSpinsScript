using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePulse : MonoBehaviour 
{
    [SerializeField] GameObject targetObject;
    [SerializeField] float expandDuration = 1.0f;
    private float currentTime = 0.0f;
    [SerializeField] Vector3 breatheIn;
    [SerializeField] Vector3 breatheOut;
    private bool breathingIn = true;
    [SerializeField] bool pulsing = false;


    private void Awake() 
    {
        if (!targetObject) {
            targetObject = this.gameObject;
        }
    }

    private void Update()
    {
        PulseUpdate();
    }

    private void PulseUpdate()
    {
        if (pulsing) {
            Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
            Vector3 startScale = breathingIn ? breatheOut : breatheIn;

            currentTime += Time.deltaTime;
            float lerpFactor = currentTime / expandDuration;

            targetObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);

            if (lerpFactor >= 1.0f) {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}