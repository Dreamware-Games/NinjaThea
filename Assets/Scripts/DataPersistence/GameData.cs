using System;
using System.Collections.Generic;
using NinjaThea.Util;

namespace NinjaThea.DataPersistence
{
    [Serializable]
    public class GameData
    {
        public List<LevelData> LevelStatus;

        public GameData()
        {
            LevelStatus = new List<LevelData>();
        }

        public void UpdateLevelStatus(LevelData levelData)
        {
            // Avoid duplicates and only write over if better time
            int index = LevelStatus.FindIndex(ld => ld.Equals(levelData));
            if (index != -1)
            {
                LevelData alreadyPassedLevelData = LevelStatus[index];
                // Replace if the new completion time is better (less)
                if (TimeFormat.Compare(levelData.CompletionTime, alreadyPassedLevelData.CompletionTime) < 0)
                {
                    LevelStatus[index] = levelData;
                }
                return;
            }

            LevelStatus.Add(levelData);
        }
    }
}
