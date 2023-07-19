namespace VctoonCore.Resources.Dtos;

public class ComicChapterDto : EntityDto<Guid>
{
    public string Name { get; set; }

    public string Path { get; set; }

    public bool IsArchive { get; set; }

    public Guid ComicId { get; set; }

    public Guid LibraryPathId { get; set; }
}