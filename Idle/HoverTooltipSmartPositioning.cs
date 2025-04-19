using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverTooltipSmartPositioning : HoverTooltip
{
    public string parentObjectName = "PowerUpAvailable";
    public string tooltipObjectName = "TooltipPowerUp";

    private GameObject tooltipGO;
    private TextMeshProUGUI tooltipText;

    void Awake()
    {
        // Trova il tooltip nella scena
        GameObject parent = GameObject.Find(parentObjectName);
        if (parent != null)
        {
            Transform tooltipTransform = parent.transform.Find(tooltipObjectName);
            if (tooltipTransform != null) {
                tooltipGO = tooltipTransform.gameObject;
                hoverCanvasGroup = tooltipTransform.GetComponent<CanvasGroup>();
                tooltipText = tooltipTransform.GetComponentInChildren<TextMeshProUGUI>();

                // Inizializza invisibile
                hoverCanvasGroup.alpha = 0f;
                tooltipGO.SetActive(false);
            } else {
                Debug.LogError("[HoverTooltipSmartPositioning]" + $"Tooltip '{tooltipObjectName}' non trovato come figlio di '{parentObjectName}'");
            }
        } else {
            Debug.LogError("[HoverTooltipSmartPositioning]" + $"Oggetto '{parentObjectName}' non trovato nella scena");
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipGO == null) return;

        tooltipGO.SetActive(true);
        base.OnPointerEnter(eventData); // usa il fade ereditato
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipGO == null) return;

        base.OnPointerExit(eventData); // usa il fade out
    }

    public void SetTooltipText(string text)
    {
        if (tooltipText != null)
            tooltipText.text = text;
    }
}