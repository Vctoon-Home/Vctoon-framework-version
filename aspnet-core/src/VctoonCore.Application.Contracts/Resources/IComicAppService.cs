
namespace VctoonCore.Resources;

public interface IComicAppService
{
    Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false);
    Task<ComicDto> GetAsync(Guid id);
    Task<PagedResultDto<ComicDto>> GetListAsync(GetComicsInput input);
    Task<ComicDto> UpdateAsync(Guid id, ComicDto input);
}