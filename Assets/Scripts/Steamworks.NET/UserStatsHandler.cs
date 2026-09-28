using NinjaThea.Managers;
using Steamworks;
using UnityEngine;

namespace NinjaThea.Steamworks.NET
{
    // Lives on the Steam Manager object, which SteamManager keeps across scene loads
    public class UserStatsHandler : Singleton<UserStatsHandler>
    {
        [SerializeField] private bool updateStats;

        public void PopAchievement(string achievementID)
        {
            if (!updateStats)
            {
                Debug.LogWarning($"Achievement {achievementID} not popped because 'updateStats' is false");
                return;
            }

            if (achievementID != null && SteamManager.Initialized)
            {
                SteamUserStats.GetAchievement(achievementID, out bool achievementUnlocked);
                if (!achievementUnlocked)
                {
                    SteamUserStats.SetAchievement(achievementID);
                    SteamUserStats.StoreStats();
                }
            }
        }
    }
}
