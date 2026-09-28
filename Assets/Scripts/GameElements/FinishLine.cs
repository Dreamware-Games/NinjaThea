using NinjaThea.Managers;
using NinjaThea.Steamworks.NET;
using UnityEngine;

namespace NinjaThea.GameElements
{
    public class FinishLine : MonoBehaviour
    {
        [SerializeField] private AudioSource finishLineCrossedSound;
        [SerializeField] private string levelCompleteAchievementID;
        [SerializeField] private bool isFinalStage;

        private bool isFinished = false;
        private bool unfinishedChecked = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            if (GameManager.Instance.TasksCompleted)
            {
                if (!isFinished)
                {
                    isFinished = true;
                    finishLineCrossedSound.Play();

                    if (UserStatsHandler.Instance != null && !string.IsNullOrEmpty(levelCompleteAchievementID))
                        UserStatsHandler.Instance.PopAchievement(levelCompleteAchievementID);

                    if (!isFinalStage) GameManager.Instance.StageComplete();
                    else GameManager.Instance.GameComplete();
                }
            }
            else
            {
                GameManager.Instance.DisplayTasksNotCompleteWarningText();
                if (UserStatsHandler.Instance != null && !unfinishedChecked)
                {
                    unfinishedChecked = true; // Do it only once per scene load
                    UserStatsHandler.Instance.PopAchievement("ACH_REACHED_EXIT_INCOMPLETE");
                }
            }
        }

        public bool IsFinished()
        {
            return isFinished;
        }
    }
}
