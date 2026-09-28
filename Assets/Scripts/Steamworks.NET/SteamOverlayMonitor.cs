using System;
using NinjaThea.Managers;
using Steamworks;

namespace NinjaThea.Steamworks.NET
{
    public class SteamOverlayMonitor : PersistentSingleton<SteamOverlayMonitor>
    {
        public static event Action<bool> OnOverlayActiveChanged;

        private Callback<GameOverlayActivated_t> overlayCallback;

        protected override void OnSingletonAwake()
        {
            overlayCallback = Callback<GameOverlayActivated_t>.Create(OnOverlayEvent);
        }

        private void OnOverlayEvent(GameOverlayActivated_t pCallback)
        {
            OnOverlayActiveChanged?.Invoke(pCallback.m_bActive != 0);
        }
    }
}
