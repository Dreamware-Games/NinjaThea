using UnityEngine;
using Steamworks;

public class UserStatsHandler : MonoBehaviour
{
    public static UserStatsHandler Instance;

    [SerializeField] private bool updateStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PopAchievement(string achievementID)
    {
        if (!updateStats)
        {
            Debug.LogWarning("Achievement not popped because 'updateStats' is false");
            return;
        }

        if (achievementID != null && SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement(achievementID, out bool achievementUnlocked);
            if (!achievementUnlocked)
            {
                SteamUserStats.SetAchievement(achievementID);
                SteamUserStats.StoreStats();
            }
        }
    }

}
