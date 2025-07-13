using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI version;

    private StageLoader stageLoader;

    private void Start()
    {
        Cursor.visible = true;
        if (version != null) version.text = Application.version;
        stageLoader = FindFirstObjectByType<StageLoader>();
    }

    public void ReloadCurrentScene()
    {
        stageLoader.LoadStageByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        stageLoader.LoadNextStage();
    }

    public void LoadScene(int index)
    {
        stageLoader.LoadStageByIndex(index);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}