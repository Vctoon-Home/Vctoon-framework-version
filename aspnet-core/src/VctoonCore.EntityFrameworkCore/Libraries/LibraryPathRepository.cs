namespace VctoonCore.Libraries;

public class LibraryPathRepository : EfCoreRepository<VctoonCoreDbContext, LibraryPath, Guid>, ILibraryPathRepository
{
    public LibraryPathRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<LibraryPath>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}