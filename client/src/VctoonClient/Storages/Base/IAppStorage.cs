namespace VctoonClient.Storages.Base;

public interface IAppStorage : ISingletonDependency
{
    public void SaveStorage();
    public void LoadStorage();
}