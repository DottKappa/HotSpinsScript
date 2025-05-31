using UnityEngine;
using Steamworks;

public class SteamOverlay : MonoBehaviour
{
    private Callback<GameOverlayActivated_t> overlayCallback;

    private void Awake()
    {
        if (FindObjectsByType<SteamOverlay>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            overlayCallback = Callback<GameOverlayActivated_t>.Create(OnOverlayActivated);
        }
    }

    private void Update()
    {
        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }

    private void OnOverlayActivated(GameOverlayActivated_t callback)
    {
        // Nessuna azione/log per ora
    }
}
