using System;
using System.IO;
using VctoonClient.Consts;
using Newtonsoft.Json;

namespace VctoonClient.Storages;

public interface IStorage : ISingletonDependency
{
    public abstract string? StorageFolder { get; set; }
    public abstract string StorageFileName { get; set; }
    public void Save();
    public void Load();
}

public abstract class StorageBase : IStorage
{
    [JsonIgnore]
    public abstract string? StorageFolder { get; set; }

    [JsonIgnore]
    public abstract string StorageFileName { get; set; }

    private string GetCurrentOsDefaultSavePath()
    {
        return Os.IsWindows || Os.IsLinux
            ? Environment.CurrentDirectory
            : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }

    public string GetSaveFilePath()
    {
        var folderPath = StorageFolder.IsNullOrEmpty() ? GetCurrentOsDefaultSavePath() : StorageFolder;
        return Path.Combine(folderPath, StorageFileName);
    }

    public void Save()
    {
        var path = GetSaveFilePath();

        if (File.Exists(path))
            File.Delete(path);

        var str = JsonConvert.SerializeObject(this);

        File.WriteAllText(path, str);
    }

    public void Load()
    {
        var path = GetSaveFilePath();

        if (!File.Exists(path))
            return;

        var json = File.ReadAllText(path);
        
        JsonConvert.PopulateObject(json, this);
    }
}