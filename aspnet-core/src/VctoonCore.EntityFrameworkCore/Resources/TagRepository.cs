namespace VctoonCore.Resources;

// TagRepository
public class TagRepository : EfCoreRepository<VctoonCoreDbContext, Tag, Guid>, ITagRepository
{
    public TagRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
