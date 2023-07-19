using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Archives;
using VctoonCore.Consts;
using VctoonCore.DataResolves;
using VctoonCore.Resources;
using Volo.Abp.Guids;

namespace VctoonCore.ArchiveDataHandlers;

public class ArchiveDataResolve : IDisposable
{
    private readonly string[] _fileFilterExtensions;

    public ArchiveDataResolve(string archiveFilePath, IEnumerable<string> fileFilterExtensions = null)
    {
        _fileFilterExtensions = fileFilterExtensions?.ToArray();
        Archive = ArchiveFactory.Open(archiveFilePath);
        ArchivePath = archiveFilePath;
    }

    public ArchiveDataResolve(IEnumerable<string> archiveFilePaths, IEnumerable<string> fileFilterExtensions = null)
    {
        archiveFilePaths = archiveFilePaths.ToArray();

        _fileFilterExtensions = fileFilterExtensions?.ToArray();
        var streams = archiveFilePaths.Select(File.OpenRead).ToList();
        Archive = ArchiveFactory.Open(streams);
        ArchivePath = Path.GetFullPath(archiveFilePaths.OrderBy(a => a).First());
    }

    public ArchiveDataResolve(IArchive archive, string archivePath, IEnumerable<string> fileFilterExtensions = null)
    {
        Archive = archive;
        ArchivePath = archivePath;
        _fileFilterExtensions = fileFilterExtensions?.ToArray();
    }

    public string ArchivePath { get; set; }

    public string ArchiveFileName
    {
        get => Path.GetFileName(ArchivePath);
    }

    public IArchive Archive { get; }

    private string[] _archiveDirectoryKeys;

    public string[] ArchiveDirectoryKeys
    {
        get
        {
            if (_archiveDirectoryKeys == null)
            {
                _archiveDirectoryKeys = Archive.Entries.Where(x => x.IsDirectory).Select(x => x.Key).ToArray();
            }

            return _archiveDirectoryKeys;
        }
    }

    private ArchiveDataTree _archiveEntries;

    public ArchiveDataTree TreeData
    {
        get
        {
            if (_archiveEntries == null)
            {
                var dataTree = BuildTree(Archive.Entries.Where(a => a.IsDirectory).Select(x => x.Key).ToList(),
                    ArchiveFileName);
                _archiveEntries = dataTree;
            }

            return _archiveEntries;
        }
    }


    public List<ComicChapter> GetComicChapters(IGuidGenerator guidGenerator,
        Guid comicId,
        Guid libraryPathId,
        ArchiveDataTree treeData = null)
    {
        var comicChapters = new List<ComicChapter>();

        treeData ??= TreeData;

        var title = Path.GetDirectoryName(treeData.Key);

        if (title.IsNullOrEmpty())
        {
            title = Path.GetFileName(treeData.Key);

            // replace ComicArchiveExtensions 
            foreach (var comicArchiveExtension in ResourceSupportFileExtensions.ComicArchiveExtensions)
            {
                title = title!.Replace(comicArchiveExtension, "");
            }
        }

        var chapter = new ComicChapter(guidGenerator.Create(), title, treeData.Key, true, comicId,
            libraryPathId);

        chapter.AddImages(treeData.Entries.Select(e => new ComicImage(guidGenerator.Create(), e.Key, chapter.Id))
            .ToList());

        if (!chapter.Images.IsNullOrEmpty())
            comicChapters.Add(chapter);

        if (!treeData.Children.IsNullOrEmpty())
        {
            foreach (var archiveDataTree in treeData.Children)
            {
                comicChapters.AddRange(GetComicChapters(guidGenerator, comicId, libraryPathId, archiveDataTree));
            }
        }

        return comicChapters;
    }

    private bool IsInFilterExtensions(string key)
    {
        if (_fileFilterExtensions == null)
        {
            return true;
        }

        return _fileFilterExtensions.Any(x => key.EndsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private ArchiveDataTree BuildTree(IEnumerable<string> paths, string rootKey)
    {
        var root = new ArchiveDataTree()
        {
            Key = rootKey,
            Entries = Archive.Entries
                .Where(x => !ArchiveDirectoryKeys.Any(k => x.Key.StartsWith(k)))
                .Where(x => !x.IsDirectory)
                .Where(x => IsInFilterExtensions(x.Key))
                .ToArray()
        };
        foreach (var path in paths)
        {
            AddNode(root, path.Split(Path.DirectorySeparatorChar));
        }

        return root;
    }

    private void AddNode(ArchiveDataTree current, IList<string> pathParts)
    {
        var part = pathParts[0];
        var child = current.Children.FirstOrDefault(n => n.Key == part);
        if (child == null)
        {
            child = new ArchiveDataTree()
            {
                Key = part,
                Entries = Archive.Entries
                    .Where(x => x.Key.StartsWith(part))
                    .Where(x => !x.IsDirectory)
                    .Where(x => IsInFilterExtensions(x.Key))
                    .Where(x => Path.GetDirectoryName(x.Key) == Path.GetDirectoryName(part))
                    .ToArray()
            };
            current.Children.Add(child);
        }

        if (pathParts.Count > 1)
        {
            AddNode(child, pathParts.Skip(1).ToArray());
        }
    }


    public void Dispose()
    {
        Archive.Dispose();
    }
}