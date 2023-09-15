using System.Threading.Tasks;
using VctoonCore.Enums;
using Volo.Abp.BackgroundJobs;

namespace VctoonCore.Libraries;

public class LibraryManager : DomainService, ILibraryManager
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryPathRepository _libraryPathRepository;

    public LibraryManager(ILibraryRepository libraryRepository,
        ILibraryPathRepository libraryPathRepository,
        IBackgroundJobManager backgroundJobManager)
    {
        _libraryRepository = libraryRepository;
        _libraryPathRepository = libraryPathRepository;
    }

    public virtual async Task<Library> CreateAsync(string name, string[] paths, LibraryType libraryType)
    {
        var library = new Library(
            GuidGenerator.Create(),
            name,
            libraryType
        );

        if (!paths.IsNullOrEmpty())
        {
            library.Paths = paths.Select(p => new LibraryPath(GuidGenerator.Create(), library.Id, p)).ToList();
        }

        await ValidateLibraryAsync(library);
        library = await _libraryRepository.InsertAsync(library);
        return library;
    }

    public virtual async Task<Library> UpdateAsync(Library library)
    {
        await ValidateLibraryAsync(library);

        library = await _libraryRepository.UpdateAsync(library);
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