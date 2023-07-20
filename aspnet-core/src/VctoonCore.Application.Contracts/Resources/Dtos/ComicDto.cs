namespace VctoonCore.Resources.Dtos;

// ComicDto

public class ComicDto : BaseResourceDto
{
    public List<ComicChapterDto> Chapters { get; set; } = new List<ComicChapterDto>();
}