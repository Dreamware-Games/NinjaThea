using UnityEngine;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{

    public static DataPersistenceManager Instance;

    [SerializeField] private bool saveGameDataOnComplete;

    public GameData SaveGameData { get; private set; }
    private IDataPersister dataPersister;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        MigrateOldSaveIfNeeded();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dataPersister = new FileDataPersister();
        LoadData();
    }

    private void MigrateOldSaveIfNeeded()
    {
        string newFilePath = Path.Combine(Application.persistentDataPath, "gamedata.ninja");
        string oldFilePath = Path.Combine(Application.persistentDataPath.Replace("Dreamware Games", "Reza Mirzaei"), "gamedata.ninja");
        if (File.Exists(oldFilePath) && !File.Exists(newFilePath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                File.Copy(oldFilePath, newFilePath);
                Debug.Log("Migrated old save file from old config to Dreamware Games.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to migrate save file: " + e);
            }
        }
    }

    public void SaveData(LevelData levelData)
    {
        if (!saveGameDataOnComplete)
        {
            Debug.LogWarning("Game not saved! Flag 'saveGameDataOnComplete is " + saveGameDataOnComplete);
            return;
        }
        if (SaveGameData == null)
        {
            SaveGameData = new GameData();
        }
        SaveGameData.UpdateLevelStatus(levelData);
        dataPersister.Save(SaveGameData);
    }

    private void LoadData()
    {
        SaveGameData = dataPersister.Load();
        if (SaveGameData == null)
        {
            SaveGameData = new GameData();
        }
    }
}