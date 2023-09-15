namespace VctoonCore.Resources;

public interface IComicImageAppService : IRemoteService
{
    Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false);
    Task<ComicImageDto> GetAsync(Guid id);
    Task<PagedResultDto<ComicImageDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<ComicImageDto> UpdateAsync(Guid id, ComicImageDto input);
}