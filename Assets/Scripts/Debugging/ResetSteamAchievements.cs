using Steamworks;
using UnityEngine;

namespace NinjaThea.Debugging
{
    public class ResetSteamAchievements : MonoBehaviour
    {
        // Editor only: never resets stats in a shipped build
        [SerializeField] private bool resetAchievements;

        private void Start()
        {
#if UNITY_EDITOR
            if (resetAchievements && SteamManager.Initialized)
            {
                Debug.LogWarning("Resetting stats and achievements!");
                SteamUserStats.ResetAllStats(true);
            }
#endif
        }
    }
}
