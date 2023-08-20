using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VctoonClient.Consts;
using Volo.Abp.IO;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VctoonClient.Storages.Base;

public abstract class AppStorageBase : IAppStorage
{
    private bool? ignore = null;

    private const string RootFolderPath = "Settings";


    // public AppStorageBase()
    // {
    // }
    //
    // protected AppStorageBase(string savePath)
    // {
    //     _savePath = savePath;
    // }

    [StorageIgnore]
    private bool IgnoreStorage
    {
        get
        {
            if (ignore == null)
                ignore = this.GetType().GetSingleAttributeOrNull<StorageIgnoreAttribute>() != null;

            return ignore.Value;
        }
    }

    protected string _savePath;

    [StorageIgnore]
    protected string SavePath
    {
        get
        {
            if (string.IsNullOrEmpty(_savePath))
                _savePath = GetSaveFilePath();

            return _savePath;
        }
        set => _savePath = value;
    }


    private string GetCurrentOsDefaultSavePath()
    {
        var dirPath = Path.Combine(Os.IsWindows || Os.IsLinux
            ? Environment.CurrentDirectory
            : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), RootFolderPath);
        DirectoryHelper.CreateIfNotExists(dirPath);

        return dirPath;
    }

    private string GetSaveFilePath()
    {
        var folderPath = GetCurrentOsDefaultSavePath();

        var fileName = this.GetType().Name.Replace("Storage", "") + ".json";

        return Path.Combine(folderPath, fileName);
    }

    public void SaveStorage()
    {
        if (IgnoreStorage)
        {
            return;
        }

        var path = SavePath;

        if (File.Exists(path))
            File.Delete(path);

        var str = JsonConvert.SerializeObject(this, new StorageConverter());

        File.WriteAllText(path, str);
    }

    public void LoadStorage()
    {
        if (IgnoreStorage)
        {
            return;
        }

        var path = SavePath;

        if (!File.Exists(path))
            return;

        var type = GetType();

        var json = File.ReadAllText(path);

        var obj = JsonConvert.DeserializeObject(json, type, new StorageConverter());

        // obj is of type this type, but the value is the value of obj, so all the values of obj need to be assigned to this

        var resolve = new AppStorageResolver();

        var members = resolve.GetRuleMembers(type);
        foreach (var member in members)
        {
            resolve.SetMemberValue(member, this, resolve.GetMemberValue(member, obj));
        }
    }

    public void ClearStorage()
    {
        var path = SavePath;
        if (File.Exists(path))
            File.Delete(path);
    }
}