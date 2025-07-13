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
    }

    private void Update()
    {
        if (!GameManager.Instance.gamePlaying) return;
        if (Input.GetButtonDown("Pause"))
        {
            if (Paused) Resume();
            else Pause();
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
