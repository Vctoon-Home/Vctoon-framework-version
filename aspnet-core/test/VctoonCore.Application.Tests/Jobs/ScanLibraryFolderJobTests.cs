using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VctoonCore.JobModels;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Jobs;
using VctoonCore.Resources;
using VctoonCore.TestDatas;
using Volo.Abp.Uow;
using Xunit;

namespace VctoonCore.Jobs;

public class ScanLibraryFolderJobTests : VctoonCoreApplicationTestBase
{
    private ScanLibraryFolderJob _libraryFolderJob;
    private IComicRepository _comicRepository;
    private IComicChapterRepository _comicChapterRepository;
    private ILibraryRepository _libraryRepository;

    private IUnitOfWorkManager _unitOfWorkManager;

    public ScanLibraryFolderJobTests()
    {
        _libraryFolderJob = GetRequiredService<ScanLibraryFolderJob>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _libraryRepository = GetRequiredService<ILibraryRepository>();
        _comicRepository = GetRequiredService<IComicRepository>();
        _comicChapterRepository = GetRequiredService<IComicChapterRepository>();
    }

    [Fact]
    public async Task Should_Execute_MyBackgroundJob_Correctly()
    {
        try
        {
            var args = new ScanLibraryFolderArgs(LibraryTestData.Id);

            _libraryFolderJob.Execute(args);
        }
        catch (Exception e)
        {
        }

        
        using var uow = _unitOfWorkManager.Begin();

        try
        {
            var comics = await (await _comicRepository.WithDetailsAsync()).ToListAsync();

            var chapters = await (await _comicChapterRepository.WithDetailsAsync()).ToListAsync();
        }
        catch (Exception e)
        {
            
        }

        await uow.CompleteAsync();
    }
}