namespace VctoonCore.Resources;

// ComicImageRepository
public class ComicImageRepository : EfCoreRepository<VctoonCoreDbContext, ComicImage, Guid>, IComicImageRepository
{
    public ComicImageRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
