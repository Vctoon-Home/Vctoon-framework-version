using System.Collections.Generic;

namespace VctoonCore.Resources.Dtos;

public abstract class BaseResourceDto : EntityDto<Guid>
{
    public string Title { get; set; }

    public double Rate { get; set; }

    public string Description { get; set; }

    public string CoverPath { get; set; }

    public string Author { get; set; }

    public long Size { get; set; }

    public int ReadCount { get; set; }


    public Guid LibraryId { get; set; }

    public virtual List<TagDto> Tags { get; set; } = new List<TagDto>();
}