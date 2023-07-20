using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace VctoonCore.Resources;

public class ComicImageAppService : CrudAppService<ComicImage, ComicImageDto, Guid>, IComicImageAppService
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.ComicImage.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.ComicImage.Default;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.ComicImage.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.ComicImage.Delete;

    public ComicImageAppService(IRepository<ComicImage, Guid> repository) : base(repository)
    {
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
    public override Task<ComicImageDto> CreateAsync(ComicImageDto input)
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