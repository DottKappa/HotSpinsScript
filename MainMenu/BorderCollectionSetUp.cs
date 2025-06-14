using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BorderCollectionSetUp : MonoBehaviour
{
    public GameObject[] waifu;

    private void OnEnable() {
        string skin = PlayerPrefs.GetString("borderSkin", "pink");
        string imagePath = "Texture/SlotSKin/Border/" + skin;
        Sprite newSprite = Resources.Load<Sprite>(imagePath);

        foreach (GameObject obj in waifu)
        {
            Transform borderTransform = obj.transform.Find("Border");
            if (borderTransform != null)
            {
                Image borderImage = borderTransform.GetComponent<Image>();
                if (borderImage != null)
                {
                    borderImage.sprite = newSprite;
                }
            }

            if (obj.name == "GeneralInfo")
            {
                Image img = obj.GetComponent<Image>();
                skin = PlayerPrefs.GetString("borderSkin", "pink");

                // Mappa skin → colore HEX
                Dictionary<string, Color32> skinColors = new Dictionary<string, Color32>
                {
                    { "green",  new Color32(0x5D, 0xF6, 0xBE, 0xFF) },
                    { "red",    new Color32(0xF6, 0x56, 0x46, 0xFF) },
                    { "pink",   new Color32(0xF4, 0x95, 0xB9, 0xFF) },
                    { "purple", new Color32(0x92, 0x63, 0xFE, 0xFF) },
                    { "blue",   new Color32(0x89, 0xBA, 0xF9, 0xFF) },
                };

                if (skinColors.TryGetValue(skin, out Color32 baseColor))
                {
                    Color.RGBToHSV(baseColor, out float h, out float s, out float _);
                    Color finalColor = Color.HSVToRGB(h, s, 0.8f); // V = 80%
                    finalColor.a = 0.6f;                           // A = 60%
                    img.color = finalColor;
                }
            }
        }
    }
}
