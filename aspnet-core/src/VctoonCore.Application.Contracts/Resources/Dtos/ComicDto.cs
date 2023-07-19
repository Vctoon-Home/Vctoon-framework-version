using System.Collections.Generic;

namespace VctoonCore.Resources.Dtos;

// ComicDto

public class ComicDto : BaseResourceDto
{
    public virtual List<ComicChapterDto> Chapters { get; set; } = new List<ComicChapterDto>();
}