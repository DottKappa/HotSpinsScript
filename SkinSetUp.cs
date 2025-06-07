using UnityEngine;
using UnityEngine.UI;

public class SkinSetUp : MonoBehaviour
{
    [Header("Slot Skin")]
    public Image slotSkin;

    [Header("Border Skin")]
    public Image borderSkin;

    [Header("Buttons Skin")]
    public Image[] buttonsSlotSkin;
    public Image[] buttonsSpinSkin;
    public Image[] buttonsTextSkin;
    public Image buttonsTextLongSkin;
    public Image buttonsSkinSelectorSkin;
    public Image buttonsIdleSkin;

    void Awake()
    {
        SetUpSlotSKin();
        SetUpBorderSKin();
        SetUpButtons();


    }

    private void SetUpSlotSKin()
    {
        string skin = PlayerPrefs.GetString("slotSkin");
        string imagePath = "Texture/SlotSKin/Slot/" + skin;
        Sprite newSprite = Resources.Load<Sprite>(imagePath);
        slotSkin.sprite = newSprite;
    }

    private void SetUpBorderSKin()
    {
        string skin = PlayerPrefs.GetString("borderSkin");
        string imagePath = "Texture/SlotSKin/Border/" + skin;
        Sprite newSprite = Resources.Load<Sprite>(imagePath);
        borderSkin.sprite = newSprite;
    }

    private void SetUpButtons()
    {
        Sprite newSprite;
        string imagePath;
        string skin = PlayerPrefs.GetString("buttonSkin");
        foreach (var item in buttonsSlotSkin)
        {
            imagePath = "Texture/SlotSKin/Buttons/" + skin + "/slotButton";
            newSprite = Resources.Load<Sprite>(imagePath);
            item.sprite = newSprite;
        }
        foreach (var item in buttonsSpinSkin)
        {
            imagePath = "Texture/SlotSKin/Buttons/" + skin + "/heart_slot";
            newSprite = Resources.Load<Sprite>(imagePath);
            item.sprite = newSprite;
        }
        foreach (var item in buttonsTextSkin)
        {
            imagePath = "Texture/SlotSKin/Buttons/" + skin + "/idleTime";
            newSprite = Resources.Load<Sprite>(imagePath);
            item.sprite = newSprite;
        }

        imagePath = "Texture/SlotSKin/Buttons/" + skin + "/longText";
        newSprite = Resources.Load<Sprite>(imagePath);
        buttonsTextLongSkin.sprite = newSprite;

        imagePath = "Texture/SlotSKin/Buttons/" + skin + "/heart_skinSelector";
        newSprite = Resources.Load<Sprite>(imagePath);
        buttonsSkinSelectorSkin.sprite = newSprite;

        imagePath = "Texture/SlotSKin/Buttons/" + skin + "/heart_idle";
        newSprite = Resources.Load<Sprite>(imagePath);
        buttonsIdleSkin.sprite = newSprite;
    }
}
