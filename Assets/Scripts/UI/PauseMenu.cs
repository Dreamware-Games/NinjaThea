using NinjaThea.Managers;
using NinjaThea.Steamworks.NET;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NinjaThea.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool Paused;

        [SerializeField] private GameObject pauseMenu;

        private void OnEnable()
        {
            SteamOverlayMonitor.OnOverlayActiveChanged += HandleOverlayActiveChanged;
        }

        private void OnDisable()
        {
            SteamOverlayMonitor.OnOverlayActiveChanged -= HandleOverlayActiveChanged;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            // Toggle once per press (started/canceled would toggle again)
            if (!context.performed) return;
            if (!GameManager.Instance.GamePlaying) return;

            if (Paused)
                Resume();
            else
                Pause();
        }

        private void HandleOverlayActiveChanged(bool steamOverlayActive)
        {
            // Only mid-run: pausing during the countdown or stage-end would freeze their coroutines
            if (!Paused && steamOverlayActive && GameManager.Instance.GamePlaying)
            {
                Pause();
            }
        }

        public void Resume()
        {
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            Paused = false;
        }

        private void Pause()
        {
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Paused = true;
        }

        public void MainMenu()
        {
            StageLoader.Instance.LoadStageByIndex(0);
        }

        public void Retry()
        {
            StageLoader.Instance.ReloadCurrentStage();
        }
    }
}
