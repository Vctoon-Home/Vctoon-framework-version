using System.IO;

namespace VctoonCore.Resources;

public class ComicImage : Entity<Guid>
{
    protected ComicImage()
    {
    }

    public ComicImage(Guid id, string path, Guid comicChapterId) : base(id)
    {
        Path = path;
        ComicChapterId = comicChapterId;
    }
    public string Path { get; protected set; }
    public Guid ComicChapterId { get; set; }
    public void SetPath(string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));
        // if (!File.Exists(path))
        //     throw new Exception(@$"${nameof(path)} is not valid path. {path} is not exist.");
        Path = path;
    }


}
