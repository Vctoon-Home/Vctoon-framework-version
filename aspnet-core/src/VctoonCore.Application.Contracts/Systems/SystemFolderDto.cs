using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace VctoonCore.Systems;

public class SystemFolderDto
{
    public SystemFolderDto()
    {
    }

    public SystemFolderDto(DirectoryInfo directoryInfo)
    {
        Name = directoryInfo.Name;
        Path = directoryInfo.FullName;
        HasChildren = directoryInfo.EnumerateDirectories().IsNullOrEmpty();
    }

    public SystemFolderDto(DriveInfo driveInfo)
    {
        Name = driveInfo.Name;
        Path = driveInfo.Name;
        HasChildren = driveInfo.RootDirectory.EnumerateDirectories().IsNullOrEmpty();
    }


    public string Name { get; set; }

    public string Path { get; set; }

    public bool HasChildren { get; set; }

    public List<SystemFolderDto> Children { get; set; }
}