using System.Collections.Generic;
using System.IO;
using System.Linq;
using VctoonCore.Consts;
using VctoonCore.Helpers;
using VctoonCore.Libraries;
using VctoonCore.Resources.DataResolves;
using Volo.Abp.Guids;

namespace VctoonCore.Resources.Handlers;

public class ComicArchiveFileScanHandler : IScanHandler
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IComicChapterRepository _comicChapterRepository;
    private readonly IComicRepository _comicRepository;


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
        var volumes = ArchiveHelper.GetArchiveVolumeFiles(new FileInfo(libraryFile.FilePath));

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

        return oldComicChapter.Images.Any(comicImage => newComicChapter.Images.All(i => i.Path != comicImage.Path));
    }
}