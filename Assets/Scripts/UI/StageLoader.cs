using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{

    public static StageLoader Instance;

    [SerializeField] private Animator crossfadeAnimator;
    [SerializeField] private float transitionTime = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        PauseMenu.Paused = false;
    }

    public void LoadNextStage()
    {
        StartCoroutine(LoadNextStageByIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadStageByIndex(int sceneIndex)
    {
        StartCoroutine(LoadNextStageByIndex(sceneIndex));
    }

    IEnumerator LoadNextStageByIndex(int sceneIndex)
    {
        crossfadeAnimator.SetTrigger("Crossfade");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
