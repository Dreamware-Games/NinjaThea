using System;
using Steamworks;
using UnityEngine;

public class SteamOverlayMonitor : MonoBehaviour
{
    public static event Action<bool> OnOverlayActiveChanged;
    public static SteamOverlayMonitor Instance { get; private set; }

    private Callback<GameOverlayActivated_t> overlayCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        overlayCallback = Callback<GameOverlayActivated_t>.Create(OnOverlayEvent);
    }

    private void OnOverlayEvent(GameOverlayActivated_t pCallback)
    {
        OnOverlayActiveChanged?.Invoke(pCallback.m_bActive != 0);
    }
}
