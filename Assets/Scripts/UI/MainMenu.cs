using NinjaThea.Managers;
using TMPro;
using UnityEngine;

namespace NinjaThea.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI version;

        private void Start()
        {
            // Stages have a GameManager that owns the cursor; menu and end scenes don't
            if (GameManager.Instance == null)
            {
                Cursor.visible = true;
                if (version != null) version.text = Application.version;
            }
        }

        public void ReloadCurrentScene()
        {
            StageLoader.Instance.ReloadCurrentStage();
        }

        public void LoadNextScene()
        {
            StageLoader.Instance.LoadNextStage();
        }

        public void LoadScene(int index)
        {
            StageLoader.Instance.LoadStageByIndex(index);
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
}
