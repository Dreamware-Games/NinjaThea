using System.Collections;
using NinjaThea.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjaThea.UI
{
    public class StageLoader : Singleton<StageLoader>
    {
        private static readonly int CrossfadeHash = Animator.StringToHash("Crossfade");

        [SerializeField] private Animator crossfadeAnimator;
        [SerializeField] private float transitionTime = 2f;

        private void Start()
        {
            Time.timeScale = 1f;
            PauseMenu.Paused = false;
        }

        public void LoadNextStage()
        {
            StartCoroutine(LoadByIndex(SceneManager.GetActiveScene().buildIndex + 1));
        }

        public void ReloadCurrentStage()
        {
            StartCoroutine(LoadByIndex(SceneManager.GetActiveScene().buildIndex));
        }

        public void LoadStageByIndex(int sceneIndex)
        {
            StartCoroutine(LoadByIndex(sceneIndex));
        }

        private IEnumerator LoadByIndex(int sceneIndex)
        {
            crossfadeAnimator.SetTrigger(CrossfadeHash);
            yield return new WaitForSecondsRealtime(transitionTime);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
