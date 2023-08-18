namespace VctoonClient.Storages.Base;

public interface IAppStorage : ISingletonDependency
{
    public void Save();
    public void Load();
}