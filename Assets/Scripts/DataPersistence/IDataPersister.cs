namespace NinjaThea.DataPersistence
{
    public interface IDataPersister
    {
        GameData Load();

        void Save(GameData gameData);
    }
}
