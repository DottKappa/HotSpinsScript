using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BorderDetailSetUp : MonoBehaviour
{
    private GameObject[] waifu;

    private void Update()
    {
        List<GameObject> foundWaifus = new List<GameObject>();
        Transform content = transform.Find("Viewport/Content");
        foreach (Transform child in content)
        {
            if (child.name == "WaifuDetail(Clone)")
            {
                foundWaifus.Add(child.gameObject);
            }
        }
        waifu = foundWaifus.ToArray();

        string skin = PlayerPrefs.GetString("borderSkin", "pink");
        string imagePath = "Texture/SlotSKin/Border/" + skin;
        Sprite newSprite = Resources.Load<Sprite>(imagePath);
        foreach (GameObject obj in waifu)
        {
            Transform borderTransform = obj.transform.Find("WaifuDetailImage/Border");
            if (borderTransform != null)
            {
                Image borderImage = borderTransform.GetComponent<Image>();
                if (borderImage != null)
                {
                    borderImage.sprite = newSprite;
                }
            }
        }
    }
}
