namespace VctoonCore.Resources;

public class ComicChapter : Entity<Guid>
{
    protected ComicChapter()
    {
    }

    public ComicChapter(Guid id, string title, string path, bool isArchive, Guid comicId, Guid libraryPathId) :
        base(id)
    {
        Title = title;
        Path = path;
        IsArchive = isArchive;
        ComicId = comicId;
        LibraryPathId = libraryPathId;
    }

    public string Title { get; protected set; }

    /// <summary>
    /// if IsArchive is true, Path is archive file path, else Path is directory path.
    /// </summary>
    public string Path { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsArchive { get; protected set; }

    // public ComicArchiveInfo ArchiveInfo { get; protected set; }

    public Guid ComicId { get; protected internal set; }
    public Guid LibraryPathId { get; protected internal set; }

    public virtual ICollection<ComicImage> Images { get; set; } = new List<ComicImage>();

    public void SetName(string name)
    {
        Check.NotNullOrEmpty(name, nameof(name));
        Title = name;
    }

    public void SetPath(string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));
        Path = path;
    }

    public void SetArchive(bool isArchive)
    {
        IsArchive = isArchive;
    }

    public void SetLibraryPathId(Guid libraryPathId)
    {
        LibraryPathId = libraryPathId;
    }

    public void SetComicId(Guid comicId)
    {
        ComicId = comicId;
    }

    public void AddImage(ComicImage image)
    {
        Check.NotNull(image, nameof(image));
        image.ComicChapterId = Id;
        Images.Add(image);
    }

    public void AddImages(IEnumerable<ComicImage> images)
    {
        Check.NotNull(images, nameof(images));
        foreach (var comicImage in images)
        {
            AddImage(comicImage);
        }
    }
    public void ClearImages()
    {
        Images.Clear();
    }
}
