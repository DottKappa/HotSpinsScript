using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarTimer : MonoBehaviour
{
    [Header("UI")]
    public Slider progressBar;
    public TMP_Text timeText;

    [Header("Timer Settings")]
    public float totalDurationInSeconds = 3600f; // 60 minuti = 3600 secondi

    private float elapsedTime = 0f;
    private float timeRemaining = 3600f;
    private IdleFileManager idleFileManager;
    private IdlePowerUpManager idlePowerUpManager;
    private UIBumpScaler uiBumpScaler;
    private WaifuChibi waifuChibi;

    void Awake()
    {
        idleFileManager = FindFirstObjectByType<IdleFileManager>();
    }

    void Start()
    {
        idlePowerUpManager = GetComponentInParent<IdlePowerUpManager>();
        Transform giftImageTransform = transform.parent.Find("Gift/GiftImage");
        uiBumpScaler = giftImageTransform.GetComponent<UIBumpScaler>();
        Transform waifuChibiTransform = transform.parent.Find("Background/WaifuChibi");
        waifuChibi = waifuChibiTransform.GetComponent<WaifuChibi>();
        UpdateTimerMultiplier();
    }

    void Update()
    {
        // Se il tempo non è ancora terminato
        if (elapsedTime < totalDurationInSeconds) {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(elapsedTime / totalDurationInSeconds);
            progressBar.value = fillAmount;

            // Calcolo del tempo rimanente
            timeRemaining = totalDurationInSeconds - elapsedTime;
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            
            // Aggiorna il testo
            timeText.text = $"{minutes:D2}:{seconds:D2}";
        } else {
            // Tempo terminato, forza il completamento della barra e fissa il testo su 00:00
            progressBar.value = 1f;
            timeText.text = "00:00";
 
            waifuChibi.StartStopWaifu(false);
            uiBumpScaler.PopUpGiftButton();
        }
    }

    public float GetTimeForNextReward()
    {
        return Mathf.Floor(timeRemaining);
    }

    public void UpdateTimerMultiplier()
    {
        string nomePadre = transform.parent.name;
        if (IdleStatic.ExistsRoom(nomePadre)) {
            totalDurationInSeconds = IdleStatic.GetRoomDurationByRoomName(nomePadre);
            Room room = idleFileManager.GetOrCreateRoomByName(nomePadre);
            totalDurationInSeconds = totalDurationInSeconds / room.TimeMultiplier;
            if (room.TimeNextReward == 0) {
                elapsedTime = 0;
            } else {
                elapsedTime = totalDurationInSeconds - (room.TimeNextReward / room.TimeMultiplier);
            }
        } else {
            totalDurationInSeconds = totalDurationInSeconds * 2;
            Debug.LogWarning("[ProgressBarTimer.cs] Non è stato possibile riconoscere il tipo di stanza. Inizializzazione di default");
        }
    }

    public void PickUpGift()
    {
        CallPowerUpCreation();
        ResetTimer();
        uiBumpScaler.HideButton();
    }

    private void CallPowerUpCreation()
    {
        string[] createdPowerUp = idlePowerUpManager.CreateRandomPowerUp();
        idleFileManager.UpdateOrCreatePowerUp(createdPowerUp[0], int.Parse(createdPowerUp[1]));
    }

    private void ResetTimer()
    {
        string nomePadre = transform.parent.name;
        Room room = idleFileManager.GetRoomByName(nomePadre);
        room.TimeNextReward = IdleStatic.GetRoomDurationByRoomName(nomePadre);
        timeRemaining = IdleStatic.GetRoomDurationByRoomName(nomePadre);
        UpdateTimerMultiplier();
        idleFileManager.SaveIdleFile();
        waifuChibi.StartStopWaifu(true);
    }
}