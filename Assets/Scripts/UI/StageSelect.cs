using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private Button[] stageButtons;
    [SerializeField] private StageLoader stageLoader;

    private void Start()
    {

        GameData gameData = DataPersistenceManager.Instance.SaveGameData;
        bool noGameData = gameData == null || gameData.LevelStatus.Count == 0;

        int lastStagePlayedIndex = 0;
        foreach (var stageButton in stageButtons)
        {
            if (noGameData)
            {
                // No game data, gray out button
                stageButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
                continue;
            }

            // Check if stage played and enable button for selection
            bool stageAlreadyPlayed = false;
            foreach (var levelStatus in gameData.LevelStatus)
            {
                if (String.Equals(levelStatus.LevelName,
                    stageButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text,
                    StringComparison.OrdinalIgnoreCase))
                {
                    lastStagePlayedIndex++;
                    stageAlreadyPlayed = true;
                    stageButton.onClick.AddListener(delegate { ReplayStage(levelStatus.StageIndex); });
                    break;
                }
            }

            // Stage not played yet, gray out the text in button
            if (!stageAlreadyPlayed)
            {
                stageButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
            }

        }

        // Add next stage, after last finished stage (a sort of continue)
        if (lastStagePlayedIndex < stageButtons.Length)
        {
            Button continueStageButton = stageButtons[lastStagePlayedIndex];
            continueStageButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            continueStageButton.onClick.AddListener(delegate { ReplayStage(lastStagePlayedIndex + 1); });
        }

        void ReplayStage(int stageIndex)
        {
            stageLoader.LoadStageByIndex(stageIndex);
        }

    }

}
