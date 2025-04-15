using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    bool hiddenInfo = false;
    public TextMeshProUGUI infoText;
    public GameObject infoBackground;

    public void HideUnhideInfo()
    {
        hiddenInfo = !hiddenInfo;

        infoText.gameObject.SetActive(hiddenInfo);
        infoBackground.SetActive(hiddenInfo);
    }
}
