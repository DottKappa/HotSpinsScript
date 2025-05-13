using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorSkin : MonoBehaviour
{
    public GameObject waifuSelector;
    public Image[] stepImages; 
    public CanvasGroup waifuCanvasGroup;

    private bool isSkinSelection = false;
    private bool isFading = false;
    private CanvasController canvasController;
    private FileManager fileManager;    

    void Start()
    {
        canvasController = FindFirstObjectByType<CanvasController>();
        fileManager = FindFirstObjectByType<FileManager>();
        SetUpImageButtons();
    }

    public void SetUpImageButtons()
    {
        string waifuName = fileManager.GetActiveWaifuName().ToString();
        bool needBlur = true;

        for (int i = 0; i < stepImages.Length; i++) {
            int step = i + 1;
            Sprite newSprite;

            if (IsSkinUnlocked(step) || needBlur) {
                string imagePath = "Texture/Waifu/" + waifuName + "/" + waifuName + "_" + step;
                newSprite = Resources.Load<Sprite>(imagePath);
                stepImages[i].GetComponent<Button>().enabled = true;
                ChangeComponentByName(stepImages[i].gameObject, "UIEffect", true);

                if (!IsSkinUnlocked(step) && needBlur) {
                    needBlur = false;
                    stepImages[i].GetComponent<Button>().enabled = false;
                } else {
                    ChangeComponentByName(stepImages[i].gameObject, "UIEffect", false);
                }
            } else {
                string imagePath = "Texture/Waifu/Lock";
                newSprite = Resources.Load<Sprite>(imagePath);
                ChangeComponentByName(stepImages[i].gameObject, "UIEffect", false);
                stepImages[i].GetComponent<Button>().enabled = false;
            }

            if (newSprite != null) {
                stepImages[i].sprite = newSprite;
            }
        }
    }

    public void SelectSkinButton()
    {
        if (isFading) return;

        isSkinSelection = !isSkinSelection;
        
        if (isSkinSelection) {
            waifuSelector.SetActive(isSkinSelection);
            StartCoroutine(FadeInCanvasGroup(waifuCanvasGroup, 0.5f));
        } else {
            StartCoroutine(FadeOutCanvasGroup(waifuCanvasGroup, 0.5f));
        }
    }

    public void StepSkinButton(int step)
    {
        string waifuName = fileManager.GetActiveWaifuName().ToString();
        if (step != 0 && IsSkinUnlocked(step)) {
            canvasController.SetWaifuImage(waifuName, waifuName + "_" + step);
        }
    }

    private bool IsSkinUnlocked(int step)
    {
        int upperStep = fileManager.GetImageStepByWaifu(fileManager.GetActiveWaifuName());
        if (step <= upperStep) return true;
        
        return false;
    }

    private void ChangeComponentByName(GameObject target, string componentName, bool isEnable)
    {
        Component[] allComponents = target.GetComponents<Component>();
        foreach (Component comp in allComponents) {
            if (comp.GetType().Name == componentName) {
                if (comp is Behaviour behaviour) {
                    behaviour.enabled = isEnable;
                }
            }
        }
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        isFading = true;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        isFading = false;
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        isFading = true;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        isFading = false;
        waifuSelector.SetActive(isSkinSelection);
    }
}
