using System;
using System.Runtime.InteropServices;

namespace VctoonClient.Consts;

public enum OsType
{
    Windows,
    Linux,
    MacOS,
    FreeBSD,
    Android,
    IOS,
    Browser
}

public class Os
{
    static Os()
    {
        IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        IsFreeBSD = RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        IsAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));
        IsIOS = RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS"));
        IsBrowser = RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
    }

    public static bool IsWindows { get; }
    public static bool IsLinux { get; }
    public static bool IsMacOS { get; }
    public static bool IsFreeBSD { get; }
    public static bool IsAndroid { get; }
    public static bool IsIOS { get; }
    public static bool IsBrowser { get; }

    public static OsType Type
    {
        get
        {
            if (IsWindows)
            {
                return OsType.Windows;
            }
            else if (IsLinux)
            {
                return OsType.Linux;
            }
            else if (IsMacOS)
            {
                return OsType.MacOS;
            }
            else if (IsFreeBSD)
            {
                return OsType.FreeBSD;
            }
            else if (IsAndroid)
            {
                return OsType.Android;
            }
            else if (IsIOS)
            {
                return OsType.IOS;
            }
            else if (IsBrowser)
            {
                return OsType.Browser;
            }
            else
            {
                throw new Exception("Unknown OS");
            }
        }
    }
}