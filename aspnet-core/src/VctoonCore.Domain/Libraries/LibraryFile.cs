using System.Diagnostics.CodeAnalysis;
using System.IO;
using VctoonCore.Consts;

namespace VctoonCore.Libraries;

public class LibraryFile : Entity<Guid>
{
    protected LibraryFile()
    {
    }


    public LibraryFile(Guid id, string fileName, string filePath, string fileExtension, long size, Guid libraryPathId,
        DateTime lastModifyTime, DateTime? lastResolveTime = null) : base(id)
    {
        SetFileName(fileName);
        SetFilePath(filePath);
        SetFileExtension(fileExtension);
        SetSize(size);
        SetLastModifyTime(lastModifyTime);
        SetLastResolveTime(lastResolveTime);
        LibraryPathId = libraryPathId;
    }

    public LibraryFile(Guid id, Guid libraryPathId, [NotNull] FileInfo fileInfo,
        DateTime? lastResolveTime = null) : base(id)
    {
        SetFileName(fileInfo.Name);
        SetFilePath(fileInfo.FullName);
        SetFileExtension(fileInfo.Extension);
        SetSize(fileInfo.Length);
        SetLastModifyTime(fileInfo.LastWriteTimeUtc);
        SetLastResolveTime(lastResolveTime);
        LibraryPathId = libraryPathId;
    }

    public string FileName { get; protected set; }

    public string FilePath { get; protected set; }

    public string FileExtension { get; protected set; }

    public long Size { get; protected set; }

    public Guid LibraryPathId { get; protected set; }

    /// <summary>
    /// using file LastWriteTimeUtc
    /// </summary>
    public DateTime LastModifyTime { get; protected set; }

    public DateTime? LastResolveTime { get; protected set; }

    public void SetFileName(string fileName)
    {
        Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        FileName = fileName;
    }

    public void SetFilePath(string filePath)
    {
        Check.NotNullOrWhiteSpace(filePath, nameof(filePath));
        // if (!File.Exists(filePath))
        //     throw new Exception(@$"${nameof(filePath)} is not valid path. {filePath} is not exist.");
        FilePath = filePath;
    }

    public void SetFileExtension(string fileExtension)
    {
        Check.NotNullOrWhiteSpace(fileExtension, nameof(fileExtension));
        FileExtension = fileExtension;
    }

    public void SetSize(long size)
    {
        Check.NotNull(size, nameof(size));
        Size = size;
    }

    public void SetLastModifyTime(DateTime lastModifyTime)
    {
        Check.NotNull(lastModifyTime, nameof(lastModifyTime));
        LastModifyTime = lastModifyTime;
    }

    public void SetLastResolveTime(DateTime? lastResolveTime)
    {
        LastResolveTime = lastResolveTime;
    }

    public bool IsChangedOrNewFile()
    {
        return LastResolveTime == null || LastModifyTime > LastResolveTime;
    }

    public bool IsImage()
    {
        return ResourceSupportFileExtensions.ComicImageExtensions.Contains(FileExtension);
    }

    public bool IsArchive()
    {
        return ResourceSupportFileExtensions.ComicArchiveExtensions.Contains(FileExtension);
    }

    public bool IsVideo()
    {
        return ResourceSupportFileExtensions.VideoExtensions.Contains(FileExtension);
    }
}