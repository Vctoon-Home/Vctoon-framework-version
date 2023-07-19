using System.Collections.Generic;
using System.IO;
using System.Linq;
using VctoonCore.ArchiveDataHandlers;
using VctoonCore.Consts;
using VctoonCore.Libraries;
using VctoonCore.Resources;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace VctoonCore.Handlers.Resources;

public class ComicArchiveFileScanHandler : IScanHandler
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IComicChapterRepository _comicChapterRepository;
    private readonly IComicRepository _comicRepository;
    static readonly List<string> AllVolumeExtensions = new List<string>();

    static ComicArchiveFileScanHandler()
    {
        Init();
    }


    public ComicArchiveFileScanHandler(IGuidGenerator guidGenerator, IComicChapterRepository comicChapterRepository,
        IComicRepository comicRepository)

    {
        _guidGenerator = guidGenerator;
        _comicChapterRepository = comicChapterRepository;
        _comicRepository = comicRepository;
    }


    public async Task Handler(LibraryPath libraryPath)
    {
        foreach (var libraryFile in libraryPath.Files.Where(f => f.IsArchive()))
        {
            var (adds, deletes, updates) = await GetArchiveFileData(libraryFile, libraryPath.Id);

            if (!deletes.IsNullOrEmpty())
                await _comicChapterRepository.DeleteManyAsync(deletes);

            if (!adds.IsNullOrEmpty())
                await _comicRepository.InsertManyAsync(adds.Select(c =>
                {
                    var comic = new Comic(_guidGenerator.Create(), c.Name,
                        libraryPath.LibraryId);

                    comic.AddChapter(c);
                    return comic;
                }));

            if (updates.IsNullOrEmpty())
                await _comicChapterRepository.UpdateManyAsync(updates);
        }
    }

    async Task<(List<ComicChapter> addChapters, List<ComicChapter> deleteChapters, List<ComicChapter> updateChapters)>
        GetArchiveFileData(LibraryFile libraryFile, Guid libraryPathId)
    {
        var volumes = await GetArchiveVolumeFiles(new FileInfo(libraryFile.FilePath));

        using var archiveResolver = new ArchiveDataResolve(volumes.Select(v => v.FullName),
            ResourceSupportFileExtensions.ComicImageExtensions);

        var dbChapters = (await _comicChapterRepository.WithDetailsAsync()).Where(c =>
            c.Path == libraryFile.FilePath && c.IsArchive && c.LibraryPathId == libraryPathId).ToList();

        var realFileSystemChapters = archiveResolver.GetComicChapters(_guidGenerator, Guid.Empty, libraryPathId);

        var deleteComicChapters = dbChapters.Where(c => realFileSystemChapters.All(s => s.Path != c.Path)).ToList();

        var addComicChapters = realFileSystemChapters.Where(c => dbChapters.All(s => s.Path != c.Path)).ToList();

        var updateComicChapters = new List<ComicChapter>();

        var checkUpdateComicChapters =
            dbChapters.Where(c => deleteComicChapters.All(dc => dc.Path != c.Path)).ToList();

        foreach (var checkUpdateComicChapter in checkUpdateComicChapters)
        {
            var realComicChapterInFileSystem =
                realFileSystemChapters.First(s => s.Path == checkUpdateComicChapter.Path);

            if (await ComicChapterIsChanged(checkUpdateComicChapter, realComicChapterInFileSystem))
            {
                checkUpdateComicChapter.SetName(realComicChapterInFileSystem.Name);
                checkUpdateComicChapter.Images.Clear();
                checkUpdateComicChapter.AddImages(realComicChapterInFileSystem.Images);
                updateComicChapters.Add(checkUpdateComicChapter);
            }
        }


        return (addComicChapters, deleteComicChapters, updateComicChapters);
    }

    async Task<bool> ComicChapterIsChanged(ComicChapter oldComicChapter, ComicChapter newComicChapter)
    {
        if (oldComicChapter.IsArchive != newComicChapter.IsArchive)
            return true;

        if (oldComicChapter.Images.Count != newComicChapter.Images.Count)
            return true;

        foreach (var comicImage in oldComicChapter.Images)
        {
            if (newComicChapter.Images.All(i => i.Path != comicImage.Path))
                return true;
        }

        return false;
    }


    /// <summary>
    /// get archive volumes and main volume files by main volume
    /// </summary>
    /// <param name="archiveFile"></param>
    /// <returns></returns>
    async Task<List<FileInfo>> GetArchiveVolumeFiles(FileInfo archiveFile)
    {
        var dirInfo = archiveFile.Directory;

        // Assume you've already obtained all FileInfo in the directory
        FileInfo[] files = dirInfo.GetFiles();

        // Create a list to store the volumes   
        List<FileInfo> volumes = new List<FileInfo>();

        // The name of the main volume, without extension
        string mainVolumeName = archiveFile.Name.Replace(archiveFile.Extension, "");


        foreach (FileInfo file in files)
        {
            // The file name without extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);

            // If the file name starts with the main volume name, and the file's extension is in the volume extension list, then it might be a volume
            if (fileNameWithoutExtension.StartsWith(mainVolumeName) && AllVolumeExtensions.Contains(file.Extension))
            {
                volumes.Add(file);
            }
        }

        // Now, the volumes list contains all the volumes
        return volumes.OrderByDescending(f => f.Name).ToList();
    }

    static void Init()
    {
        // Possible extensions for RAR volumes
        List<string> rarVolumeExtensions = Enumerable.Range(0, 100).Select(i => $".r{i:D2}").ToList();
        rarVolumeExtensions.Insert(0, ".rar"); // Insert .rar as the main volume

        // Possible extensions for ZIP volumes
        List<string> zipVolumeExtensions = Enumerable.Range(1, 100).Select(i => $".z{i:D2}").ToList();
        zipVolumeExtensions.Add(".zip"); // .zip is the last volume

        // Possible extensions for 7z volumes
        List<string> sevenZVolumeExtensions = Enumerable.Range(1, 100).Select(i => $".7z.{i:D3}").ToList();

        // Merge all volume extensions
        AllVolumeExtensions.AddRange(rarVolumeExtensions);
        AllVolumeExtensions.AddRange(zipVolumeExtensions);
        AllVolumeExtensions.AddRange(sevenZVolumeExtensions);
    }
}