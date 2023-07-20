using Volo.Abp;

namespace VctoonCore.Resources;

public class ComicChapterAppService : CrudAppService<ComicChapter, ComicChapterDto, Guid>, IComicChapterAppService
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.ComicChapter.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.ComicChapter.Default;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.ComicChapter.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.ComicChapter.Delete;

    public ComicChapterAppService(IComicChapterRepository repository) : base(repository)
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
    public override Task<ComicChapterDto> CreateAsync(ComicChapterDto input)
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