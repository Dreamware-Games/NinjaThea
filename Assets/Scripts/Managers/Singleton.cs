using UnityEngine;

namespace NinjaThea.Managers
{
    // Scene singleton: a duplicate destroys itself, the instance lives and dies with its scene
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = (T)this;
            OnSingletonAwake();
        }

        // Only runs on the surviving instance
        protected virtual void OnSingletonAwake() { }
    }
}
