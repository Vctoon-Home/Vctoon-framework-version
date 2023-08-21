using System.IO;

namespace VctoonCore.Libraries;

public class LibraryPath : Entity<Guid>
{
    protected LibraryPath()
    {
    }


    public LibraryPath(Guid id, Guid libraryId, string path, DateTime? lastModifyTime = null,
        DateTime? lastResolveTime = null, Guid? parentId = null) : base(id)
    {
        LibraryId = libraryId;
        Path = path;
        LastModifyTime = lastModifyTime;
        LastResolveTime = lastResolveTime;
        ParentId = parentId;
    }

    public LibraryPath(Guid id, Guid libraryId, DirectoryInfo directoryInfo,
        DateTime? lastResolveTime = null, Guid? parentId = null) : base(id)
    {
        LibraryId = libraryId;
        Path = directoryInfo.FullName;
        LastModifyTime = directoryInfo.LastWriteTimeUtc;
        LastResolveTime = lastResolveTime;
        ParentId = parentId;
    }


    public Guid LibraryId { get; protected internal set; }

    // public virtual Library Library { get; }
    public string Path { get; protected set; }

    /// <summary>
    /// using directory LastWriteTimeUtc
    /// </summary>
    public DateTime? LastModifyTime { get; protected set; }

    public DateTime? LastResolveTime { get; protected set; }

    public Guid? ParentId { get; protected internal set; }

    public virtual List<LibraryPath> Children { get; } = new List<LibraryPath>();

    public virtual List<LibraryFile> Files { get; } = new List<LibraryFile>();

    public void SetPath(string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));
        if (!Directory.Exists(path))
            throw new Exception(@$"${nameof(path)} is not valid path. {path} is not exist.");
        Path = path;
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

    // add child
    public void AddChild(LibraryPath child)
    {
        Check.NotNull(child, nameof(child));

        if (Children.Any(c => c.Id == child.Id))
            return;

        child.ParentId = Id;
        Children.Add(child);
    }

    // add children
    public void AddChildren(List<LibraryPath> children)
    {
        Check.NotNull(children, nameof(children));
        foreach (var child in children)
        {
            Children.Add(child);
        }
    }


    // add file
    public void AddFile(LibraryFile file)
    {
        Check.NotNull(file, nameof(file));

        if (Files.Any(c => c.Id == file.Id))
            return;

        Files.Add(file);
    }

    // add files
    public void AddFiles(List<LibraryFile> files)
    {
        Check.NotNull(files, nameof(files));
        foreach (var file in files)
        {
            Files.Add(file);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lastResolveTime">default value is DateTime.UtcNow</param>
    public void SetAllFilesLastResolveTime(DateTime? lastResolveTime = null)
    {
        if (lastResolveTime == null)
            lastResolveTime = DateTime.UtcNow;
        foreach (var file in Files)
        {
            file.SetLastResolveTime(lastResolveTime);
        }
    }


    public bool ExistInRealFileSystem()
    {
        return Directory.Exists(Path);
    }
}