namespace NinjaThea.Managers
{
    // Singleton that survives scene loads
    public abstract class PersistentSingleton<T> : Singleton<T> where T : PersistentSingleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            if (Instance == this) DontDestroyOnLoad(gameObject);
        }
    }
}
