using Steamworks;
using UnityEngine;

namespace NinjaThea.Steamworks.NET
{
    public class PromoLink : MonoBehaviour
    {
        [SerializeField] private string storeAppID;

        public void OpenStorePage()
        {
            if (SteamManager.Initialized && uint.TryParse(storeAppID, out uint appID))
            {
                SteamFriends.ActivateGameOverlayToStore(
                    new AppId_t(appID),
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_None
                );
            }
        }
    }
}
