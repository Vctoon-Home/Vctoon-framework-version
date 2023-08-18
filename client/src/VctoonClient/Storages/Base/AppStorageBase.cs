using System;
using System.IO;
using Newtonsoft.Json;
using VctoonClient.Consts;
using Volo.Abp.IO;

namespace VctoonClient.Storages.Base;

public abstract class AppStorageBase : IAppStorage
{
    // [JsonIgnore]
    // public abstract string? StorageFolder { get; set; }
    //
    // [JsonIgnore]
    // public abstract string StorageFileName { get; set; }

    private string GetCurrentOsDefaultSavePath()
    {
        var dirPath = Path.Combine(Os.IsWindows || Os.IsLinux
            ? Environment.CurrentDirectory
            : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Settings");
        DirectoryHelper.CreateIfNotExists(dirPath);

        return dirPath;
    }

    public string GetSaveFilePath()
    {
        var folderPath = GetCurrentOsDefaultSavePath();
        return Path.Combine(folderPath, this.GetType().Name.Replace("AppStorage", "") + ".json");
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