namespace VctoonCore.Resources;

// ComicRepository
public class ComicRepository : EfCoreRepository<VctoonCoreDbContext, Comic, Guid>, IComicRepository
{
    public ComicRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Comic>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}