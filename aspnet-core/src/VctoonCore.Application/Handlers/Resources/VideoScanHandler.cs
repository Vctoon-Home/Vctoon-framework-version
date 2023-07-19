using VctoonCore.Libraries;

namespace VctoonCore.Handlers.Resources;

public class VideoScanHandler : IScanHandler
{
    public Task Handler(LibraryPath libraryPath)
    {
        return Task.CompletedTask;
    }
}