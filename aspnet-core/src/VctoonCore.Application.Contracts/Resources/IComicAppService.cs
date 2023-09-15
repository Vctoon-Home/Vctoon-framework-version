namespace VctoonCore.Resources;

public interface IComicAppService : IRemoteService
{
    Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false);
    Task<ComicDto> GetAsync(Guid id);
    Task<PagedResultDto<ComicDto>> GetListAsync(GetComicListInput input);
    Task<ComicDto> UpdateAsync(Guid id, ComicDto input);
}