using VctoonCore.Enums;
using VctoonCore.Libraries;

namespace VctoonCore.Resources.Handlers;

public class VideoScanHandler : IScanHandler
{
    public LibraryType SupportLibraryType { get; set; } = LibraryType.Video;
    public Task Handler(LibraryPath libraryPath)
    {
        return Task.CompletedTask;
    }
}
