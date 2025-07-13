using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public bool tasksCompleted;
    public bool gamePlaying;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private GameObject itemsContainer;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private GameObject enemiesContainer;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private GameObject hudContainer;
    [SerializeField] private GameObject stageCompleteContainer;
    [SerializeField] private GameObject gameCompleteContainer;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI tasksNotCompleteWarningText;
    [SerializeField] private TextMeshProUGUI bestText;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI loadingNextStageText;
    [SerializeField] private TextMeshProUGUI endingGameText;
    [SerializeField] private String customStageText;

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;
    // Oh dear... these two have to match.
    // The string countdown step must have the corresponding audio clip in another array, with the same index.
    [SerializeField] private string[] countdownSteps;
    [SerializeField] private AudioSource[] countdownStepsAudio;

    private int numTotalItems;
    private string itemName;
    private int numItemsCollected;
    private int numTotalEnemies;
    private string enemyName;
    private int numdEnemiesKilled;
    private float timeToAppear = 3f;
    private float timeWhenDisappear;
    private float startTime;
    private float elapsedTime;
    private TimeSpan timePlaying;
    private string currentSceneName;
    private int currentSceneIndex;
    private int secondsToWaitBeforeLoadNextStage = 10;

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
        currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        itemName = itemsContainer.transform.name;
        numTotalItems = itemsContainer.transform.childCount;
        numItemsCollected = 0;
        itemText.text = $"{itemName}: {numItemsCollected}/{numTotalItems}";

        enemyName = enemiesContainer.transform.name;
        numTotalEnemies = enemiesContainer.transform.childCount;
        numdEnemiesKilled = 0;
        enemyText.text = $"{enemyName}: {numdEnemiesKilled}/{numTotalEnemies}";

        timeText.text = "Time: 00:00.00";
        SetBestText();
        countdownText.text = "";
        loadingNextStageText.text = "";
        endingGameText.text = "";

        if (string.IsNullOrEmpty(customStageText))
            stageText.text = SceneManager.GetActiveScene().name;
        else
            stageText.text = customStageText;

        tasksCompleted = false;
        gamePlaying = false;
        Cursor.visible = false;
        StartCoroutine(CountdownToBeginGame());
    }

    private void Update()
    {
        if (gamePlaying)
        {
            elapsedTime = Time.time - startTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timeText.text = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            if (tasksNotCompleteWarningText.enabled && (Time.time >= timeWhenDisappear))
            {
                tasksNotCompleteWarningText.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator CountdownToBeginGame()
    {
        for (int i = 0; i < countdownSteps.Length; i++)
        {
            string step = countdownSteps[i];
            AudioSource audio = countdownStepsAudio[i];
            if (i == countdownSteps.Length - 1)
            {
                countdownText.fontSize = 168;
                countdownText.fontStyle = FontStyles.Italic | FontStyles.Bold;
                BeginGame();
            }
            countdownText.text = step;
            audio.Play();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(.5f);
        countdownText.gameObject.SetActive(false);
    }

    private void BeginGame()
    {
        gamePlaying = true;
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
        numdEnemiesKilled++;
        enemyText.text = $"{enemyName}: {numdEnemiesKilled}/{numTotalEnemies}";
        UpdateTasksCompleted();
    }

    private void UpdateTasksCompleted()
    {
        tasksCompleted = (numItemsCollected >= numTotalItems) && (numdEnemiesKilled >= numTotalEnemies);
    }

    public void StageComplete()
    {
        Transform stageCompleteTimeText = stageCompleteContainer.transform.Find("Stage Complete Time Text");
        PrepareStageCompletion(stageCompleteTimeText.gameObject.GetComponent<TextMeshProUGUI>());
        stageCompleteContainer.SetActive(true);
        StartCoroutine(LoadNextStage());
    }

    public void GameComplete()
    {
        Transform gameCompleteTimeText = gameCompleteContainer.transform.Find("Game Complete Time Text");
        PrepareStageCompletion(gameCompleteTimeText.gameObject.GetComponent<TextMeshProUGUI>());
        gameCompleteContainer.SetActive(true);
        StartCoroutine(EndGame());
    }

    private void PrepareStageCompletion(TextMeshProUGUI completedTimeText)
    {
        gamePlaying = false;
        string completionTime = timePlaying.ToString("mm':'ss'.'ff");
        Cursor.visible = true;
        backgroundMusic.gameObject.SetActive(false);
        completedTimeText.text = "Time: " + completionTime;
        LevelData levelData = new LevelData(currentSceneName, completionTime, currentSceneIndex);
        if (DataPersistenceManager.Instance != null) DataPersistenceManager.Instance.SaveData(levelData);
    }

    IEnumerator LoadNextStage()
    {
        for (int i = secondsToWaitBeforeLoadNextStage; i >= 0; i--)
        {
            loadingNextStageText.text = "Loading next stage in " + i + "...";
            yield return new WaitForSeconds(1f);
        }
        StageLoader.Instance.LoadNextStage();
    }

    IEnumerator EndGame()
    {
        for (int i = secondsToWaitBeforeLoadNextStage; i >= 0; i--)
        {
            endingGameText.text = "Ending game in " + i + "...";
            yield return new WaitForSeconds(1f);
        }
        StageLoader.Instance.LoadNextStage();
    }

}
