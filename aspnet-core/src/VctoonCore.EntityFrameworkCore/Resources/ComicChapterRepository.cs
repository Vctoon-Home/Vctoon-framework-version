namespace VctoonCore.Resources;

// ComicChapterRepository
public class ComicChapterRepository : EfCoreRepository<VctoonCoreDbContext, ComicChapter, Guid>, IComicChapterRepository
{
    public ComicChapterRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<ComicChapter>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}