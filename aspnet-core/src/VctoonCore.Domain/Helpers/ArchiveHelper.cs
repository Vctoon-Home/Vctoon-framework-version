using System.IO;

namespace VctoonCore.Helpers;

public class ArchiveHelper
{
    static ArchiveHelper()
    {
        // Possible extensions for RAR volumes
        List<string> rarVolumeExtensions = Enumerable.Range(0, 100).Select(i => $".r{i:D2}").ToList();
        rarVolumeExtensions.Insert(0, ".rar"); // Insert .rar as the main volume

        // Possible extensions for ZIP volumes
        List<string> zipVolumeExtensions = Enumerable.Range(1, 100).Select(i => $".z{i:D2}").ToList();
        zipVolumeExtensions.Add(".zip"); // .zip is the last volume

        // Possible extensions for 7z volumes
        List<string> sevenZVolumeExtensions = Enumerable.Range(1, 100).Select(i => $".7z.{i:D3}").ToList();

        // Merge all volume extensions
        AllVolumeExtensions.AddRange(rarVolumeExtensions);
        AllVolumeExtensions.AddRange(zipVolumeExtensions);
        AllVolumeExtensions.AddRange(sevenZVolumeExtensions);
    }
    static readonly List<string> AllVolumeExtensions = new List<string>();
    public static List<FileInfo> GetArchiveVolumeFiles(string archiveFilePath)
    {
        if (!File.Exists(archiveFilePath))
            return null;

        return GetArchiveVolumeFiles(new FileInfo(archiveFilePath));
    }
    /// <summary>
    /// get archive volumes and main volume files by main volume
    /// </summary>
    /// <param name="archiveFile"></param>
    /// <returns></returns>
    public static List<FileInfo> GetArchiveVolumeFiles(FileInfo archiveFile)
    {
        var dirInfo = archiveFile.Directory;

        // Assume you've already obtained all FileInfo in the directory
        FileInfo[] files = dirInfo.GetFiles();

        // Create a list to store the volumes   
        List<FileInfo> volumes = new List<FileInfo>();

        // The name of the main volume, without extension
        string mainVolumeName = archiveFile.Name.Replace(archiveFile.Extension, "");


        foreach (FileInfo file in files)
        {
            // The file name without extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);

            // If the file name starts with the main volume name, and the file's extension is in the volume extension list, then it might be a volume
            if (fileNameWithoutExtension.StartsWith(mainVolumeName) && AllVolumeExtensions.Contains(file.Extension))
            {
                volumes.Add(file);
            }
        }

        // Now, the volumes list contains all the volumes
        return volumes.OrderByDescending(f => f.Name).ToList();
    }
}