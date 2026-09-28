using System;

namespace NinjaThea.DataPersistence
{
    [Serializable]
    public class LevelData
    {
        public string LevelName;
        public string CompletionTime;
        public int StageIndex;

        public LevelData(string levelName, string completionTime, int stageIndex)
        {
            LevelName = levelName;
            CompletionTime = completionTime;
            StageIndex = stageIndex;

        }

        public override int GetHashCode()
        {
            return LevelName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            LevelData levelData = obj as LevelData;
            return levelData != null
                && string.Equals(LevelName, levelData.LevelName);
        }

        public override string ToString()
        {
            return LevelName + ": " + CompletionTime;
        }
    }
}
