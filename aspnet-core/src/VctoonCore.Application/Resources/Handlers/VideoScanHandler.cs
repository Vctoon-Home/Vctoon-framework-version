using VctoonCore.Libraries;

namespace VctoonCore.Resources.Handlers;

public class VideoScanHandler : IScanHandler
{
    public Task Handler(LibraryPath libraryPath)
    {
        return Task.CompletedTask;
    }
}