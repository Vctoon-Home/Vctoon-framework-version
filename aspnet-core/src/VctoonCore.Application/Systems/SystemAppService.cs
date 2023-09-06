using System.IO;
using System.Linq;

namespace VctoonCore.Systems;

public class SystemAppService : VctoonCoreAppService, ISystemAppService
{
    public async Task<List<SystemFolderDto>> GetSystemFolder(string? path = null)
    {
        var res = new List<SystemFolderDto>();

        if (path.IsNullOrEmpty())
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (var driveInfo in allDrives)
            {
                if (driveInfo.IsReady)
                    res.Add(new SystemFolderDto(driveInfo));
            }
        }
        else
        {
            var directoryInfo = new DirectoryInfo(path);

            foreach (var directory in directoryInfo.GetDirectories()
                         .Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden)))
            {
                res.Add(new SystemFolderDto(directory));
            }
        }

        return res;
    }
}