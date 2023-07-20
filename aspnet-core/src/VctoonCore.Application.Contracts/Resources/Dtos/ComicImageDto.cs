namespace VctoonCore.Resources.Dtos;

public class ComicImageDto : EntityDto<Guid>
{
    public string ArchivePath { get; protected set; }
    public string Path { get; protected set; }
    public Guid ComicChapterId { get; set; }
}