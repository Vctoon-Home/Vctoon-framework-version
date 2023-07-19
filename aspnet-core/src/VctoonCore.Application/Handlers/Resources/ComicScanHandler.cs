using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using VctoonCore.Consts;
using VctoonCore.Libraries;
using VctoonCore.Resources;
using Volo.Abp.Guids;
using Volo.Abp.Http;

namespace VctoonCore.Handlers.Resources;

public class ComicScanHandler : IScanHandler
{
    private readonly IComicChapterRepository _comicChapterRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IComicRepository _comicRepository;

    public ComicScanHandler(IComicChapterRepository comicChapterRepository, IGuidGenerator guidGenerator,
        IComicRepository comicRepository)
    {
        _comicChapterRepository = comicChapterRepository;
        _guidGenerator = guidGenerator;
        _comicRepository = comicRepository;
    }


    public async Task Handler(LibraryPath libraryPath)
    {
        var comicChapter = (await _comicChapterRepository.WithDetailsAsync())
            .FirstOrDefault(c => c.LibraryPathId == libraryPath.Id && !c.IsArchive);

        if (comicChapter == null)
        {
            if (!libraryPath.Files.Any(f => f.IsImage()))
            {
                return;
            }
            await CreateComicByLibraryPath(libraryPath);
        }
        else
        {
            await UpdateComicChapter(libraryPath, comicChapter);
        }
    }


    public Task CreateComicByLibraryPath(LibraryPath libraryPath)
    {
        var title = Path.GetDirectoryName(libraryPath.Path);

        var comic = new Comic(_guidGenerator.Create(), title, libraryPath.LibraryId);

        var comicChapter = new ComicChapter(_guidGenerator.Create(), title, libraryPath.Path, false, comic.Id,
            libraryPath.Id);

        AddNewImageFilesByLibraryPathAsync(libraryPath, comicChapter);
        comic.AddChapter(comicChapter);

        return _comicRepository.InsertAsync(comic);
    }


    public Task UpdateComicChapter(LibraryPath libraryPath,
        ComicChapter comicChapter)
    {
        RemoveNotExistImage(libraryPath, comicChapter);
        AddNewImageFilesByLibraryPathAsync(libraryPath, comicChapter);
        return _comicChapterRepository.UpdateAsync(comicChapter);
    }

    private async void AddNewImageFilesByLibraryPathAsync(LibraryPath libraryPath,
        ComicChapter comicChapter)
    {
        if (!libraryPath.ExistInRealFileSystem())
            return;

        var files = libraryPath.Files
            .Where(f => f.IsChangedOrNewFile() && f.IsImage())
            .ToList();

        comicChapter.AddImages(files.Select(f => new ComicImage(_guidGenerator.Create(), f.FilePath, comicChapter.Id)));
    }

    private void RemoveNotExistImage(LibraryPath libraryPath,
        ComicChapter comicChapter)
    {
        if (!libraryPath.ExistInRealFileSystem() ||
            !libraryPath.Files.Any(f => f.IsImage()))
        {
            comicChapter.ClearImages();
            return;
        }

        comicChapter.Images.RemoveAll(img => File.Exists(img.Path));
    }
}