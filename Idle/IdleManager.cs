using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IdleManager : MonoBehaviour
{
    //[Header("GameObj")]
    public GameObject[] roomsGameObj;

    private float fadeDuration = 1f;
    private IdleFileManager idleFileManager;
    bool isHidingUnhiding = false;

    void Start()
    {
        idleFileManager = FindFirstObjectByType<IdleFileManager>();

        string[] allRooms = IdleStatic.GetAllRooms();
        for (int i = 0; i < allRooms.Length; i++) {
            Room room = idleFileManager.GetRoomByName(allRooms[i]);
            if (room != null) {
                GameObject roomGameObj = GetRoomGameObjByName(allRooms[i]);
                if (roomGameObj != null) {
                    roomGameObj.SetActive(true);
                    DestroyUnlockableNeedOnButtonByRoomName(allRooms[i]);
                    Image imageComponent = GetImageComponentByGameObj(roomGameObj);
                    string waifuName = idleFileManager.GetActiveWaifuByRoomName(allRooms[i]);
                    Sprite sprite = GetSpriteByWaifuName(waifuName);
                    imageComponent.sprite = sprite;
                }
            }
        }

        DisableAllVisibility();
        HideUnhideRoomButton(IdleStatic.GetBasicRoom());
    }

    private GameObject GetRoomGameObjByName(string roomName)
    {
        foreach (GameObject room in roomsGameObj) {
            if (room.name == roomName) {
                return room;
            }
        }

        return null;
    }

    public void HideUnhideRoomButton(string roomName)
    {
        if (!isHidingUnhiding) {
            isHidingUnhiding = true;
            GameObject roomGameObj = GetRoomGameObjByName(roomName);
            if (roomGameObj == null) { isHidingUnhiding = false; return; }
            if (roomGameObj == GetActiveRoom()) { isHidingUnhiding = false; return; }

            // se è il gamobj è disattivo devo vedere se lo posso attivare con un unlockable
            if (!roomGameObj.activeSelf) {
                ActiveRoom(roomGameObj);
            }

            CanvasGroup cg = roomGameObj.GetComponent<CanvasGroup>();
            if (cg == null)  { isHidingUnhiding = false; return; }
            DisableAllVisibility();
            StartCoroutine(InvertVisibility(cg));
        }
    }

    private IEnumerator InvertVisibility(CanvasGroup canvasGroup)
    {
        float targetAlpha = canvasGroup.alpha == 0f ? 1f : 0f;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < fadeDuration) {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        canvasGroup.interactable = targetAlpha == 1f;
        canvasGroup.blocksRaycasts = targetAlpha == 1f;
        isHidingUnhiding = false;
    }

    private void DisableAllVisibility()
    {
        foreach (GameObject room in roomsGameObj) {
            CanvasGroup cg = room.GetComponent<CanvasGroup>();
            if (cg != null && cg.alpha != 0f) {
                StartCoroutine(FadeOut(cg));
            }
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < fadeDuration) {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    private void ActiveRoom(GameObject roomGameObj)
    {
        int unlockable = idleFileManager.GetNumberOfUnlockableRoom();
        float unlockableNeeded = IdleStatic.GetUnlockableNeededByRoomName(roomGameObj.name);
        // Controllo se è un .5, quindi se ha bisogno di tutte le altre stanze sbloccate
        if (Mathf.Abs(unlockableNeeded % 1f - 0.5f) < 0.0001f) {
            if (!CheckUnlockedRoomByRoomNameLocked(roomGameObj.name)) {
                isHidingUnhiding = false;
                throw new InvalidOperationException("[IdleManager.cs] Impossibile sbloccare la stanza [" + roomGameObj.name + "] non hai sbloccato le stanze precedenti");
            }
        }
        if (unlockable < (int)unlockableNeeded) {
            isHidingUnhiding = false;
            // TODO -> mostrarlo anche a schermo in qualche modo
            throw new InvalidOperationException("[IdleManager.cs] Impossibile sbloccare la stanza [" + roomGameObj.name + "] non hai abbastanza unlockable");
        }

        idleFileManager.UseUnlockableRoom((int)unlockableNeeded);
        roomGameObj.SetActive(true);
        DestroyUnlockableNeedOnButtonByRoomName(roomGameObj.name);
    }

    private bool CheckUnlockedRoomByRoomNameLocked(string roomName)
    {
        // Controlla che tutte le stanze diverse da quella in input siano attive
        string[] roomNames = IdleStatic.GetAllRooms();
        bool allActive = true;

        for (int i = 0; i < roomNames.Length; i++) {
            GameObject roomGameObj = GetRoomGameObjByName(roomNames[i]);
            if (roomGameObj == null) {
                isHidingUnhiding = false;
                throw new InvalidOperationException("[IdleManager.cs] Impossibile trovare la stanza [" + roomNames[i] + "]");
            }
            if (!roomGameObj.activeSelf && roomGameObj.name != roomName) {
                allActive = false;
            } 
        }

        return allActive;
    }

    private void DestroyUnlockableNeedOnButtonByRoomName(string roomName)
    {
        string path = $"RoomsButtons/{roomName}Button/UnlockNeeded";
        Transform unlockNeeded = transform.Find(path);

        if (unlockNeeded != null) {
            Destroy(unlockNeeded.gameObject);
            Debug.Log($"[IdleManager.cs] UnlockNeeded distrutto per la stanza: {roomName}");
        }
    }

    private GameObject GetActiveRoom()
    {
        foreach (GameObject room in roomsGameObj) {
            CanvasGroup cg = room.GetComponent<CanvasGroup>();
            if (cg != null && cg.alpha == 1f) {
                return room;
            }
        }

        return null;
    }

    public void SetWaifuInActiveRoom(string waifuName)
    {
        if (!Enum.TryParse<Waifu>(waifuName, ignoreCase: true, out var waifu)) {
            Debug.LogError("[IdleManager.cs] Impossibile settare la waifu per la scena. Waifu non riconosciuta");
            return;
        }

        Sprite sprite = GetSpriteByWaifuName(waifuName);
        GameObject roomActive = GetActiveRoom();
        Image imageComponent = GetImageComponentByGameObj(roomActive);

        if (imageComponent.sprite == sprite) {
            return; // Lo sprite da applicare è uguale a quello attualmente in uso. Nessun cambiamento necessario
        }

        StartCoroutine(FadeOutAndInSprite(imageComponent, sprite));
    }

    private IEnumerator FadeOutAndInSprite(Image imageComponent, Sprite newSprite)
    {
        CanvasGroup cg = imageComponent.GetComponent<CanvasGroup>();
        if (cg == null) {
            cg = imageComponent.gameObject.AddComponent<CanvasGroup>(); // Aggiungi CanvasGroup se non esiste
        }

        fadeDuration = 0.2f;
        yield return StartCoroutine(FadeOut(cg));

        imageComponent.sprite = newSprite;
        fadeDuration = 0.3f;
        yield return StartCoroutine(InvertVisibility(cg));
        
        fadeDuration = 1f;
    }

    private Image GetImageComponentByGameObj(GameObject gameObject) 
    {
        if (gameObject == null) {
            throw new ArgumentNullException(nameof(gameObject), "[IdleManager.cs] Impossibile ottenere il gameobj dal nome [" + gameObject.name + "]");
        }

        Transform background = gameObject.transform.Find("Background");
        if (background == null) {
            throw new InvalidOperationException("[IdleManager.cs] Non ho trovato 'Background' tra i figli diretti");
        }
        Transform waifuChibi = background.transform.Find("WaifuChibi");
        if (waifuChibi == null) {
            throw new InvalidOperationException("[IdleManager.cs] Non ho trovato 'WaifuChibi' tra i figli di 'Background'");
        }

        Image imageComponent = waifuChibi.GetComponent<Image>();    
        if (imageComponent == null) {
            throw new InvalidOperationException("[IdleManager.cs] Non ho trovato il componente image in 'WaifuChibi'");
        }

        return imageComponent;
    }

    private Sprite GetSpriteByWaifuName(string waifuName)
    {
        Sprite sprite = Resources.Load<Sprite>("Texture/Waifu/" + waifuName + "/Chibi/" + waifuName + "_walking");
        if (sprite == null) {
            sprite = Resources.Load<Sprite>("Texture/Waifu/" + waifuName + "/Chibi/" + waifuName + "_hidle");
        }
        if (sprite == null) {
            throw new InvalidOperationException("[IdleManager.cs] Impossibile trovare lo sprite in [Texture/Waifu/" + waifuName + "/Chibi/]");
        }

        return sprite;
    }

    public string GetWaifuActiveByRoomName(string roomName)
    {
        GameObject roomGameObj = GetRoomGameObjByName(roomName);
        Image imageComponent = GetImageComponentByGameObj(roomGameObj);
        
        string spriteName = imageComponent.sprite.name;
        string[] waifuNames = Enum.GetNames(typeof(Waifu));

        foreach (string waifuName in waifuNames) {
            if (spriteName.Contains(waifuName)) {
                return waifuName;
            }
        }

        return null;
    }
}
