namespace VctoonCore.Consts;

public class ResourceSupportFileExtensions
{
    public static string[] ComicArchiveExtensions { get; } = new[]
    {
        ".zip", ".rar", ".cbz"
    };

    public static string[] ComicImageExtensions { get; } = new[]
    {
        ".jpg", ".png", ".jpeg", ".bmp", ".gif", ".webp"
    };

    public static string[] VideoExtensions { get; } = new[]
    {
        ".mp4", ".mkv", ".avi", ".wmv", ".mov", ".flv", ".rmvb", ".rm", ".3gp", ".ts"
    };

    public static string[] GetAllSupportFileExtensions()
    {
        return ComicArchiveExtensions
            .Concat(ComicImageExtensions)
            .Concat(VideoExtensions)
            .ToArray();
    }
}