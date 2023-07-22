using System.Linq;
using VctoonCore.JobModels;
using VctoonCore.Libraries.Dtos;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace VctoonCore.Libraries;

/// <summary>
/// 
/// </summary>
public class LibraryAppService : CrudAppService<Library, LibraryDto, Guid, LibraryGetListInput, CreateUpdateLibraryDto,
        CreateUpdateLibraryDto>,
    ILibraryAppService
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.Library.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.Library.Default;
    protected override string CreatePolicyName { get; set; } = VctoonCorePermissions.Library.Create;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.Library.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.Library.Delete;

    private readonly ILibraryRepository _repository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILibraryManager _libraryManager;
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IBackgroundJobStore _backgroundJobStore;

    public LibraryAppService(ILibraryRepository repository,
        IGuidGenerator guidGenerator,
        ILibraryManager libraryManager,
        IBackgroundJobManager backgroundJobManager,
        IBackgroundJobStore backgroundJobStore) : base(repository)
    {
        _repository = repository;
        _guidGenerator = guidGenerator;
        _libraryManager = libraryManager;
        _backgroundJobManager = backgroundJobManager;
        _backgroundJobStore = backgroundJobStore;
    }

    public async Task SyncLibraryAsync(Guid id)
    {
        var library = await Repository.GetAsync(id);

        if (library == null)
            throw new BusinessException("Library is not found");

        // TODO: check the job is running

        if (!library.Paths.IsNullOrEmpty())
            await _backgroundJobManager.EnqueueAsync(new ScanLibraryFolderArgs(library.Id), BackgroundJobPriority.High);
    }

    protected override async Task<IQueryable<Library>> CreateFilteredQueryAsync(LibraryGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            ;
    }

    public override async Task<LibraryDto> CreateAsync(CreateUpdateLibraryDto input)
    {
        await CheckGetPolicyAsync();
        var library = await _libraryManager.CreateAsync(input.Name, input.Paths, input.LibraryType);
        return await MapToGetOutputDtoAsync(library);
    }

    public override async Task<LibraryDto> UpdateAsync(Guid id, CreateUpdateLibraryDto input)
    {
        await CheckUpdatePolicyAsync();
        var library = await GetEntityByIdAsync(id);
        await MapToEntityAsync(input, library);

        if (!input.Paths.IsNullOrEmpty())
        {
            foreach (var inputPath in input.Paths)
            {
                if (library.Paths.Any(p => p.Path == inputPath))
                    continue;

                library.Paths.Add(new LibraryPath(_guidGenerator.Create(), library.Id, inputPath));
            }

            var pathsToRemove = library.Paths.Where(p => !input.Paths.Contains(p.Path)).ToList();
            foreach (var path in pathsToRemove)
            {
                library.Paths.Remove(path);
            }
        }

        library = await _libraryManager.UpdateAsync(library);
        return await MapToGetOutputDtoAsync(library);
    }

    protected override async Task<Library> GetEntityByIdAsync(Guid id)
    {
        var library = (await Repository.WithDetailsAsync()).First(e => e.Id == id);

        return library;
    }
}
