using System.Collections.Generic;
using System.IO;
using System.Linq;
using VctoonCore.ArchiveDataHandlers;
using VctoonCore.Consts;
using VctoonCore.Handlers;
using VctoonCore.Handlers.Resources;
using VctoonCore.JobModels;
using VctoonCore.Libraries;
using VctoonCore.Resources;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace VctoonCore.Jobs;

public class ScanLibraryFolderJob : BackgroundJob<ScanLibraryFolderArgs>, ITransientDependency
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IEnumerable<IScanHandler> _resourceHandlers;

    public ScanLibraryFolderJob(
        ILibraryRepository libraryRepository,
        IGuidGenerator guidGenerator,
        IEnumerable<IScanHandler> resourceHandlers)
    {
        _libraryRepository = libraryRepository;
        _guidGenerator = guidGenerator;
        _resourceHandlers = resourceHandlers;
    }


    private ScanLibraryFolderArgs args { get; set; }

    [UnitOfWork]
    public override async void Execute(ScanLibraryFolderArgs args)
    {
        this.args = args;
        var query = await _libraryRepository.WithDetailsAsync();
        var library = query.FirstOrDefault(l => l.Id == args.LibraryId);

        #region Only Scan Library Path Structure

        Console.WriteLine("Only Scan Library Path Structure");
        // scan library path and files
        foreach (var libraryPath in library.Paths)
        {
            await ScanDirectoryStructureAndFiles(libraryPath);
        }

        #endregion

        #region Scan Changed LibraryPath and LibraryFile

        Console.WriteLine("Scan Changed LibraryPath and LibraryFile");
        // scan library path and files
        foreach (var libraryPath in library.Paths)
        {
            await ResolveDirectoryFiles(libraryPath);
        }
        await _libraryRepository.UpdateAsync(library);
        #endregion
    }

    async Task ScanDirectoryStructureAndFiles(LibraryPath libraryPath)
    {
        if (!libraryPath.ExistInRealFileSystem())
            return;

        DirectoryInfo dirInfo = new DirectoryInfo(libraryPath.Path);

        // if directory is changed
        if (dirInfo.LastWriteTimeUtc != libraryPath.LastResolveTime) // check files in this directory
        {
            var fileExtensions = ResourceSupportFileExtensions.GetAllSupportFileExtensions();

            var dirFiles = dirInfo.GetFiles()
                .Where(f => fileExtensions.Contains(f.Extension))
                .ToList();

            // remove not exists files and remove updated files
            libraryPath.Files.RemoveAll(file =>
                !Path.Exists(file.FilePath) || File.GetLastWriteTimeUtc(file.FilePath) != file.LastResolveTime);

            // add new files
            libraryPath.Files.AddRange(dirFiles.Where(f => libraryPath.Files.All(file => file.FilePath != f.FullName))
                .Select(f => new LibraryFile(_guidGenerator.Create(), f.Name, f.FullName, f.Extension, f.Length,
                    libraryPath.Id, f.LastWriteTimeUtc)));
        }

        var childrenDirectories = dirInfo.GetDirectories();

        if (!childrenDirectories.IsNullOrEmpty())
        {
            // remove not exists directories
            libraryPath.Children.RemoveAll(child =>
                !Directory.Exists(child.Path));

            // add new directories
            var newChildren = childrenDirectories
                .Where(d =>
                    libraryPath.Children
                        .All(child => child.Path != d.FullName)
                )
                .Select(d =>
                    new LibraryPath(_guidGenerator.Create(), libraryPath.LibraryId, d.FullName, d.LastWriteTimeUtc))
                .ToList();

            foreach (var child in newChildren)
            {
                await ScanDirectoryStructureAndFiles(child);
            }

            libraryPath.Children.AddRange(newChildren);
        }

        libraryPath.SetLastModifyTime(dirInfo.LastWriteTimeUtc);
    }

    async Task ResolveDirectoryFiles(LibraryPath libraryPath)
    {
        if (!libraryPath.ExistInRealFileSystem())
            return;

        foreach (var libraryPathChild in libraryPath.Children)
            await ResolveDirectoryFiles(libraryPathChild);


        foreach (var resourceHandler in _resourceHandlers)
        {
            await resourceHandler.Handler(libraryPath);
        }

        libraryPath.SetAllFilesLastResolveTime();
    }
}