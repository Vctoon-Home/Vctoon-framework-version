namespace VctoonCore.Resources;

// VideoRepository
public class VideoRepository : EfCoreRepository<VctoonCoreDbContext, Video, Guid>, IVideoRepository
{
    public VideoRepository(IDbContextProvider<VctoonCoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}