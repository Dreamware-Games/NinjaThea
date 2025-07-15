using UnityEngine;

public class FinishLine : MonoBehaviour
{

    [SerializeField] private AudioSource finishLineCrossedSound;
    [SerializeField] private string levelCompleteAchievementID;
    [SerializeField] private bool isFinalStage;

    private bool isFinished = false;
    private bool unifinishedChecked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.TasksCompleted)
        {
            if (!isFinished)
            {
                isFinished = true;
                finishLineCrossedSound.Play();

                if (UserStatsHandler.Instance != null && levelCompleteAchievementID != null)
                    UserStatsHandler.Instance.PopAchievement(levelCompleteAchievementID);

                if (!isFinalStage) GameManager.Instance.StageComplete();
                else GameManager.Instance.GameComplete();

            }
        }
        else
        {
            GameManager.Instance.DisplayTasksNotCompleteWarningText();
            if (UserStatsHandler.Instance != null && levelCompleteAchievementID != null && !unifinishedChecked)
            {
                unifinishedChecked = true; // Do it only once
                UserStatsHandler.Instance.PopAchievement("ACH_REACHED_EXIT_INCOMPLETE");
            }
        }

    }
    public bool IsFinalStage()
    {
        return isFinalStage;
    }

    public bool IsFinished()
    {
        return isFinished;
    }

}
