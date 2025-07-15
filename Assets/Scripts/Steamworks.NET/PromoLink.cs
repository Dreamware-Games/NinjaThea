using Steamworks;
using UnityEngine;

public class PromoLink : MonoBehaviour
{
    [SerializeField] private string storeAppID;

    public void OpenStorePage()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToStore(
                new AppId_t(uint.Parse(storeAppID)),
                EOverlayToStoreFlag.k_EOverlayToStoreFlag_None
            );
        }
    }
}
