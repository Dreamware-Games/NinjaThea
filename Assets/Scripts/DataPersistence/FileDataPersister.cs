using System;
using System.IO;
using UnityEngine;

namespace NinjaThea.DataPersistence
{
    public class FileDataPersister : IDataPersister
    {

        private string filePath = Path.Combine(Application.persistentDataPath, "gamedata.ninja");

        public void Save(GameData gameData)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                string gameDataJson = JsonUtility.ToJson(gameData, true);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(gameDataJson);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        public GameData Load()
        {
            GameData loadedGameData = null;
            if (File.Exists(filePath))
            {
                try
                {
                    string gameDataJson = null;
                    using (FileStream stream = new FileStream(filePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            gameDataJson = reader.ReadToEnd();
                        }
                    }
                    if (gameDataJson == null)
                    {
                        return null;
                    }
                    loadedGameData = JsonUtility.FromJson<GameData>(gameDataJson);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return null;
                }
            }
            return loadedGameData;
        }

    }
}
