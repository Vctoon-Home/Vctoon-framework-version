using System;

namespace VctoonClient.Storages;

public class AppStorageSavingHandler
{
    internal static Action OnApplicationExit { get; set; }

    public static void SaveStorage()
    {
        OnApplicationExit?.Invoke();
    }
}