using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using SceneManagement = UnityEngine.SceneManagement.SceneManager;

public class IdlePowerUpManager : MonoBehaviour
{
    public GameObject prefab;
    private IdleFileManager idleFileManager;
    Room room;

    void Start()
    {
        idleFileManager = FindFirstObjectByType<IdleFileManager>();

        string roomName = transform.name;
        if (IdleStatic.ExistsRoom(roomName)) {
            room = idleFileManager.GetOrCreateRoomByName(roomName);
        }
    }

    public string[] CreateRandomPowerUp()
    {
        int newAvailability = 0;
        string rarity = GetRarity();
        string powerUpTitle = IdleStatic.GetRandomPowerUp(rarity);
        string powerUpDesc = IdleStatic.GetPowerUpDescriptionByTitle(powerUpTitle);

        Transform padre = transform.parent;
        Transform content = padre.Find("PowerUpAvailable/Scroll View/Viewport/Content");

        if (content == null) { Debug.LogError("[IdlePowerUpManager.cs] Content non trovato"); return new string[] {}; }

        bool trovato = false;

        foreach (Transform child in content) {
            if (child.name.Contains(powerUpTitle)) {
                var tooltipComponent = child.GetComponent<HoverTooltipSmartPositioning>();
                if (tooltipComponent != null) {
                    tooltipComponent.SetTooltipText(powerUpDesc);
                    newAvailability = tooltipComponent.UpdateAvailability(1);
                    trovato = true;
                    break;
                }
            }
        }

        if (!trovato) {
            CreateNewPowerUp(powerUpTitle, 1);
            newAvailability = 1;
        }

        return new string[] { powerUpTitle, newAvailability.ToString() };
    }

    public void CreateNewPowerUp(string powerUpTitle, int availability)
    {
        Transform padre = transform.parent;
        Transform content = padre.Find("PowerUpAvailable/Scroll View/Viewport/Content");
        if (content == null) { throw new InvalidOperationException("[IdlePowerUpManager.cs] Content non trovato"); }

        GameObject powerUp = Instantiate(prefab, content);
        powerUp.name = powerUpTitle;
        string powerUpDesc = IdleStatic.GetPowerUpDescriptionByTitle(powerUpTitle);

        var tooltipComponent = powerUp.GetComponent<HoverTooltipSmartPositioning>();
        if (tooltipComponent != null) {
            tooltipComponent.SetTooltipText(powerUpDesc);
            tooltipComponent.UpdateAvailability(availability);
        }
        Button buttonComponent = powerUp.GetComponent<Button>();
        if (buttonComponent != null) {
            if (SceneManagement.GetActiveScene().name == "Slot") {
                buttonComponent.enabled = true;
            } else {
                buttonComponent.enabled = false;
            }
        }

        Image imageComponent = powerUp.GetComponent<Image>();
        if (imageComponent == null) {
            imageComponent = powerUp.GetComponentInChildren<Image>();
        }
        Sprite sprite = Resources.Load<Sprite>("Texture/IdlePowerUp/" + powerUpTitle);
        if (sprite != null && imageComponent != null) {
            imageComponent.sprite = sprite;
        }
    }

    private string GetRarity()
    {
        float[] weights = IdleStatic.GetWeightsByRoomLv(room.Lv);
        string[] rarities = IdleStatic.GetRarities();
        float totalWeight = 0f;
        foreach (float w in weights)
            totalWeight += w;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentSum = 0f;

        for (int i = 0; i < rarities.Length; i++)
        {
            currentSum += weights[i];
            if (randomValue <= currentSum)
                return rarities[i];
        }

        return rarities[0]; // fallback, shouldn't happen
    }
}
