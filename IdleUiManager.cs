using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IdleUiManager : MonoBehaviour
{
    [Header("Room 1")]
    public TextMeshProUGUI nameRoom1;
    public TextMeshProUGUI timerRoom1;

    [Header("Room 2")]
    public TextMeshProUGUI nameRoom2;
    public TextMeshProUGUI timerRoom2;

    [Header("Slot Controller")]
    public GameObject CanvasSlotContainer;
    public SceneManager sceneManager;

    [Header("Idle")]
    public GameObject idleGameObj;
    public TextMeshProUGUI buttonText;

    [Header("Door")]
    public GameObject door;

    private string[] timers;
    private string[] rooms;
    private bool isIdleVisible = false;


    void Start()
    {
        InvokeRepeating("AnalizeRoomsTimer", 0f, 0.5f);
    }

    public void idleButton()
    {
        if (sceneManager.GetIsRolling()) return;

        if (isIdleVisible) door.SetActive(true);
        if (!isIdleVisible) door.SetActive(false);

        bool isTogglingToIdle = !isIdleVisible;

        ToggleCanvasVisibility(isTogglingToIdle);
        isIdleVisible = isTogglingToIdle;
        buttonText.text = isIdleVisible ? "Slot" : "Idle";
    }

    private void ToggleCanvasVisibility(bool showIdle)
    {
        CanvasGroup cgSlot = CanvasSlotContainer.GetComponent<CanvasGroup>();
        CanvasGroup cgIdle = idleGameObj.GetComponent<CanvasGroup>();

        // Fade in/out for Slot and Idle
        StartCoroutine(FadeCanvasGroup(cgSlot, 0.5f, showIdle ? 0f : 1f));
        StartCoroutine(FadeCanvasGroup(cgIdle, 0.5f, showIdle ? 1f : 0f));

        if (showIdle) {
            sceneManager.EmptySlotMatrix();
        }
    }

    private void AnalizeRoomsTimer()
    {
        string[] rooms = IdleStatic.GetAllRooms();
        int i = 0;
        timers = new string[rooms.Length];
        this.rooms = new string[rooms.Length];

        foreach (string room in rooms) {
            if (TryGetRoomObject(room, out GameObject roomObj) && IsRoomActive(roomObj)) {
                timers[i] = GetRoomTimerText(roomObj);
                this.rooms[i] = room;
                i++;
            }
        }

        if (i == 0) {
            return;
        }

        Array.Resize(ref timers, i);
        Array.Resize(ref this.rooms, i);
        SortTimersAndRooms();
        UpdateUI();
    }

    private bool TryGetRoomObject(string roomName, out GameObject roomObj)
    {
        roomObj = null;
        GameObject idleObj = GameObject.Find("Idle");
        if (idleObj == null) {
            return false;
        }

        Transform roomTransform = idleObj.transform.Find(roomName);
        if (roomTransform == null) {
            Debug.LogError($"[IdleUiManager] Room '{roomName}' not found under Idle.");
            return false;
        }

        roomObj = roomTransform.gameObject;
        return true;
    }

    private bool IsRoomActive(GameObject roomObj)
    {
        return roomObj != null && roomObj.activeInHierarchy;
    }

    private string GetRoomTimerText(GameObject roomObj)
    {
        if (roomObj == null || !roomObj.activeInHierarchy)
            return "00:00";

        Transform timerTransform = roomObj.transform.Find("ProgressObj/Timer");
        if (timerTransform == null)
        {
            Debug.LogError($"[IdleUiManager] Timer not found in {roomObj.name}");
            return "00:00";
        }

        TextMeshProUGUI timerTMP = timerTransform.GetComponent<TextMeshProUGUI>();
        return timerTMP.text;
    }

    private void SortTimersAndRooms()
    {
        Array.Sort(timers, rooms, new TimerComparer());
    }

    private class TimerComparer : System.Collections.IComparer
    {
        public int Compare(object a, object b)
        {
            string timerA = (string)a;
            string timerB = (string)b;

            // Converti i timer a secondi per confrontarli
            return TimerToSeconds(timerA).CompareTo(TimerToSeconds(timerB));
        }
    }

    private static int TimerToSeconds(string timer)
    {
        if (string.IsNullOrEmpty(timer)) return int.MaxValue;

        string[] parts = timer.Split(':');
        if (parts.Length != 2) return int.MaxValue;

        int min = int.TryParse(parts[0], out var m) ? m : 0;
        int sec = int.TryParse(parts[1], out var s) ? s : 0;

        return min * 60 + sec;
    }

    private void UpdateUI()
    {
        if (timers.Length >= 1) {
            nameRoom1.text = rooms[0];
            timerRoom1.text = timers[0];
        } else {
            nameRoom1.text = "???";
            timerRoom1.text = "00:00";
        }

        if (timers.Length >= 2) {
            nameRoom2.text = rooms[1];
            timerRoom2.text = timers[1];
        } else {
            nameRoom2.text = "???";
            timerRoom2.text = "00:00";
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float duration, float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        // Se stai facendo un FadeOut, disabilita l'interazione e i raycast alla fine
        if (targetAlpha == 0f) {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else if (targetAlpha == 1f) {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
