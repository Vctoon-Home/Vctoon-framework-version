using System.Threading.Tasks;
using VctoonCore.JobModels;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace VctoonCore.Libraries;

public class LibraryManager : DomainService, ILibraryManager
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryPathRepository _libraryPathRepository;
    private readonly IBackgroundJobManager _backgroundJobManager;

    public LibraryManager(ILibraryRepository libraryRepository,
        ILibraryPathRepository libraryPathRepository,
        IBackgroundJobManager backgroundJobManager)
    {
        _libraryRepository = libraryRepository;
        _libraryPathRepository = libraryPathRepository;
        _backgroundJobManager = backgroundJobManager;
    }

    public virtual async Task<Library> CreateAsync(string name, string[] paths)
    {
        var library = new Library(
            GuidGenerator.Create(),
            name
        );

        if (!paths.IsNullOrEmpty())
        {
            library.Paths = paths.Select(p => new LibraryPath(GuidGenerator.Create(), library.Id, p)).ToList();
        }

        await ValidateLibraryAsync(library);


        library = await _libraryRepository.InsertAsync(library);

        await _backgroundJobManager.EnqueueAsync(new ScanLibraryFolderArgs(library.Id));

        return library;
    }

    public virtual async Task<Library> UpdateAsync(Library library)
    {
        await ValidateLibraryAsync(library);

        library = await _libraryRepository.UpdateAsync(library);

        await _backgroundJobManager.EnqueueAsync(new ScanLibraryFolderArgs(library.Id));

        return library;
    }

    public virtual async Task<Library> SetLibraryNameAsync(Guid id, string name)
    {
        var library = await _libraryRepository.GetAsync(id);
        library.SetName(name);
        await ValidateLibraryAsync(library);
        return await _libraryRepository.UpdateAsync(library);
    }

    public virtual Task DeleteAsync(Library library)
    {
        return _libraryRepository.DeleteAsync(library);
    }

    protected virtual async Task ValidateLibraryAsync(Library library)
    {
        await CheckNameAsync(library);
        // await CheckPathsAsync(library);
    }

    protected virtual async Task CheckNameAsync(Library library)
    {
        var existingLibrary = await _libraryRepository.FindByNameAsync(library.Name);
        if (existingLibrary != null && existingLibrary.Id != library.Id)
        {
            // TODO: Fix localization
            throw new BusinessException("DuplicateLibraryName").WithData("Name", library.Name);
        }
    }
}