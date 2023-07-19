using Microsoft.EntityFrameworkCore;

namespace VctoonCore.Resources;

[Owned]
public class ComicArchiveInfo
{
    protected ComicArchiveInfo()
    {
    }

    public ComicArchiveInfo(long fileSize, DateTime lastModifyTime)
    {
        FileSize = fileSize;
        LastModifyTime = lastModifyTime;
    }

    public long FileSize { get; protected set; }
    public DateTime LastModifyTime { get; protected set; }

    public void SetFileSize(long fileSize)
    {
        Check.Range(fileSize, nameof(fileSize), 0, long.MaxValue);
        FileSize = fileSize;
    }

    public void SetLastModifyTime(DateTime lastModifyTime)
    {
        Check.NotNull(lastModifyTime, nameof(lastModifyTime));
        LastModifyTime = lastModifyTime;
    }
}