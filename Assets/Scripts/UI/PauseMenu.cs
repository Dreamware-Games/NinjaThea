using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public static bool Paused;

    [SerializeField] private GameObject pauseMenu;

    private StageLoader stageLoader;

    private void Start()
    {
        stageLoader = FindFirstObjectByType<StageLoader>();
        SteamOverlayMonitor.OnOverlayActiveChanged += HandleOverlayActiveChanged;
    }

    private void OnDisable()
    {
        SteamOverlayMonitor.OnOverlayActiveChanged -= HandleOverlayActiveChanged;
    }

    public void OnPause()
    {
        if (!GameManager.Instance.GamePlaying) return;

        if (Paused)
            Resume();
        else
            Pause();
    }

    private void HandleOverlayActiveChanged(bool steamOverlayActive)
    {
        if (!Paused && steamOverlayActive)
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
        stageLoader.LoadStageByIndex(0);
    }

    public void Retry()
    {
        stageLoader.LoadStageByIndex(SceneManager.GetActiveScene().buildIndex);
    }

}