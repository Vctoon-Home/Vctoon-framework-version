namespace VctoonCore.Resources;

public interface IComicChapterAppService
{
    Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false);
    Task<ComicChapterDto> GetAsync(Guid id);
    Task<PagedResultDto<ComicChapterDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    Task<ComicChapterDto> UpdateAsync(Guid id, ComicChapterDto input);
}