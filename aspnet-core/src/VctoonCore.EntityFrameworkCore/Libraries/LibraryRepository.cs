namespace VctoonCore.Libraries;

public class LibraryRepository : EfCoreRepository<VctoonCoreDbContext, Library, Guid>, ILibraryRepository
{
    public LibraryRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Library>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public async Task<Library> FindByNameAsync(string name)
    {
        return await (await GetQueryableAsync()).Where(x => x.Name == name).FirstOrDefaultAsync();
    }
}