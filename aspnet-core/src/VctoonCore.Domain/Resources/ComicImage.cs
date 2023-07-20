using System.IO;
using JetBrains.Annotations;
using SharpCompress.Archives;
using VctoonCore.Helpers;

namespace VctoonCore.Resources;

public class ComicImage : Entity<Guid>
{
    protected ComicImage()
    {
    }

    public ComicImage(Guid id, string path, Guid comicChapterId, string archivePath = null) : base(id)
    {
        Path = path;
        ArchivePath = archivePath;
        ComicChapterId = comicChapterId;
    }

    public string ArchivePath { get; protected set; }
    public string Path { get; protected set; }
    public Guid ComicChapterId { get; set; }

    public void SetPath(string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));
        // if (!File.Exists(path))
        //     throw new Exception(@$"${nameof(path)} is not valid path. {path} is not exist.");
        Path = path;
    }


    [CanBeNull]
    public Stream GetImageStream()
    {
        if (!ArchivePath.IsNullOrEmpty())
        {
            var volumes = ArchiveHelper.GetArchiveVolumeFiles(ArchivePath);
            var streams = volumes.Select(v => v.OpenRead()).ToList();
            var archive = ArchiveFactory.Open(streams);

            var entry = archive.Entries.FirstOrDefault(x => x.Key == Path);
            return entry?.OpenEntryStream();
        }
        else
        {
            return File.OpenRead(Path);
        }
    }
}