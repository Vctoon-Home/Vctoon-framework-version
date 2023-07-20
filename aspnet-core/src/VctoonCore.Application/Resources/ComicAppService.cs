using System.Linq;
using Volo.Abp;

namespace VctoonCore.Resources;

public class ComicAppService : CrudAppService<Comic, ComicDto, Guid, GetComicsInput>, IComicAppService
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.Comic.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.Comic.Default;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.Comic.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.Comic.Delete;

    public ComicAppService(IComicRepository repository) : base(repository)
    {
    }

    protected override async Task<Comic> GetEntityByIdAsync(Guid id)
    {
        return (await Repository.WithDetailsAsync(x => x.Chapters)).FirstOrDefault(x => x.Id == id);
    }


    public async Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false)
    {
        await base.DeleteAsync(id);

        // TODO: Delete in real file system
        if (deleteInRealFileSystem)
        {
        }
    }


    [Obsolete]
    [RemoteService(false)]
    public override Task<ComicDto> CreateAsync(ComicDto input)
    {
        return base.CreateAsync(input);
    }

    [Obsolete]
    [RemoteService(false)]
    public override Task DeleteAsync(Guid id)
    {
        return base.DeleteAsync(id);
    }
}