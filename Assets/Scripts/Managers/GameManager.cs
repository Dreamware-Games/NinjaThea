using System;
using System.Collections;
using NinjaThea.DataPersistence;
using NinjaThea.UI;
using NinjaThea.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjaThea.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Serializable]
        public struct CountdownStep
        {
            public string text;
            public AudioSource audio;
        }

        public static event Action OnGameStarted;

        [HideInInspector] public bool TasksCompleted;
        [HideInInspector] public bool GamePlaying;

        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private GameObject itemsContainer;
        [SerializeField] private TextMeshProUGUI itemText;
        [SerializeField] private GameObject enemiesContainer;
        [SerializeField] private TextMeshProUGUI enemyText;
        [SerializeField] private GameObject stageCompleteContainer;
        [SerializeField] private GameObject gameCompleteContainer;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI tasksNotCompleteWarningText;
        [SerializeField] private TextMeshProUGUI bestText;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private TextMeshProUGUI loadingNextStageText;
        [SerializeField] private TextMeshProUGUI endingGameText;
        [SerializeField] private string customStageText;

        [Header("Countdown")]
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private CountdownStep[] countdownSteps;

        private static readonly WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
        private static readonly WaitForSeconds waitHalfSecond = new WaitForSeconds(.5f);

        private int numTotalItems;
        private string itemName;
        private int numItemsCollected;
        private int numTotalEnemies;
        private string enemyName;
        private int numEnemiesKilled;
        private float timeToAppear = 3f;
        private float timeWhenDisappear;
        private float startTime;
        private float elapsedTime;
        // "Time: mm:ss.ff", rewritten in place when the shown hundredths change
        private readonly char[] timeChars = "Time: 00:00.00".ToCharArray();
        private int shownHundredths = -1;
        private string currentSceneName;
        private int currentSceneIndex;
        private int secondsToWaitBeforeLoadNextStage = 10;

        private void Start()
        {
            currentSceneName = SceneManager.GetActiveScene().name;
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            itemName = itemsContainer.transform.name;
            numTotalItems = itemsContainer.transform.childCount;
            numItemsCollected = 0;
            itemText.text = $"{itemName}: {numItemsCollected}/{numTotalItems}";

            enemyName = enemiesContainer.transform.name;
            numTotalEnemies = enemiesContainer.transform.childCount;
            numEnemiesKilled = 0;
            enemyText.text = $"{enemyName}: {numEnemiesKilled}/{numTotalEnemies}";

            timeText.text = "Time: 00:00.00";
            SetBestText();
            countdownText.text = "";
            loadingNextStageText.text = "";
            endingGameText.text = "";

            if (string.IsNullOrEmpty(customStageText))
                stageText.text = SceneManager.GetActiveScene().name;
            else
                stageText.text = customStageText;

            TasksCompleted = false;
            GamePlaying = false;
            Cursor.visible = false;
            StartCoroutine(CountdownToBeginGame());
        }

        private void Update()
        {
            if (GamePlaying)
            {
                elapsedTime = Time.time - startTime;
                UpdateTimeText();
                if (tasksNotCompleteWarningText.enabled && (Time.time >= timeWhenDisappear))
                {
                    tasksNotCompleteWarningText.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateTimeText()
        {
            int hundredths = (int)(elapsedTime * 100f);
            if (hundredths == shownHundredths) return;
            shownHundredths = hundredths;
            TimeFormat.Write(elapsedTime, timeChars, 6);
            timeText.SetCharArray(timeChars);
        }

        private IEnumerator CountdownToBeginGame()
        {
            for (int i = 0; i < countdownSteps.Length; i++)
            {
                CountdownStep step = countdownSteps[i];
                if (i == countdownSteps.Length - 1)
                {
                    countdownText.fontSize = 168;
                    countdownText.fontStyle = FontStyles.Italic | FontStyles.Bold;
                    BeginGame();
                }
                countdownText.text = step.text;
                step.audio.Play();
                yield return waitOneSecond;
            }
            yield return waitHalfSecond;
            countdownText.gameObject.SetActive(false);
        }

        private void BeginGame()
        {
            GamePlaying = true;
            OnGameStarted?.Invoke();
            startTime = Time.time;
            backgroundMusic.gameObject.SetActive(true);
        }

        private void SetBestText()
        {
            bestText.text = "Best: --";
            if (DataPersistenceManager.Instance == null) return;
            GameData gameData = DataPersistenceManager.Instance.SaveGameData;
            if (gameData == null || gameData.LevelStatus.Count == 0) return;

            foreach (var levelStatus in gameData.LevelStatus)
            {
                if (levelStatus.LevelName.Equals(currentSceneName))
                {
                    bestText.text = "Best: " + levelStatus.CompletionTime;
                    break;
                }
            }
        }

        public void DisplayTasksNotCompleteWarningText()
        {
            tasksNotCompleteWarningText.gameObject.SetActive(true);
            timeWhenDisappear = Time.time + timeToAppear;
        }

        public void ItemCollected()
        {
            numItemsCollected++;
            itemText.text = $"{itemName}: {numItemsCollected}/{numTotalItems}";
            UpdateTasksCompleted();
        }

        public void EnemyKilled()
        {
            numEnemiesKilled++;
            enemyText.text = $"{enemyName}: {numEnemiesKilled}/{numTotalEnemies}";
            UpdateTasksCompleted();
        }

        private void UpdateTasksCompleted()
        {
            TasksCompleted = (numItemsCollected >= numTotalItems) && (numEnemiesKilled >= numTotalEnemies);
        }

        public void StageComplete()
        {
            Complete(stageCompleteContainer, "Stage Complete Time Text", loadingNextStageText, "Loading next stage in ");
        }

        public void GameComplete()
        {
            Complete(gameCompleteContainer, "Game Complete Time Text", endingGameText, "Ending game in ");
        }

        private void Complete(GameObject container, string timeTextName, TextMeshProUGUI countdownLabel, string countdownPrefix)
        {
            TextMeshProUGUI completedTimeText = container.transform.Find(timeTextName).GetComponent<TextMeshProUGUI>();
            PrepareStageCompletion(completedTimeText);
            container.SetActive(true);
            StartCoroutine(CountdownThenLoadNextStage(countdownLabel, countdownPrefix));
        }

        private void PrepareStageCompletion(TextMeshProUGUI completedTimeText)
        {
            GamePlaying = false;
            string completionTime = TimeFormat.Format(elapsedTime);
            Cursor.visible = true;
            backgroundMusic.gameObject.SetActive(false);
            completedTimeText.text = "Time: " + completionTime;
            LevelData levelData = new LevelData(currentSceneName, completionTime, currentSceneIndex);
            if (DataPersistenceManager.Instance != null) DataPersistenceManager.Instance.SaveData(levelData);
        }

        private IEnumerator CountdownThenLoadNextStage(TextMeshProUGUI countdownLabel, string countdownPrefix)
        {
            for (int i = secondsToWaitBeforeLoadNextStage; i >= 0; i--)
            {
                countdownLabel.text = countdownPrefix + i + "...";
                yield return waitOneSecond;
            }
            StageLoader.Instance.LoadNextStage();
        }
    }
}
