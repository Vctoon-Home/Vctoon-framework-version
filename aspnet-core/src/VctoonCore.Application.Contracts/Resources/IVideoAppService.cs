namespace VctoonCore.Resources;

public interface IVideoAppService: IRemoteService
{
    Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false);
    Task<VideoDto> GetAsync(Guid id);
    Task<PagedResultDto<VideoDto>> GetListAsync(GetVideosInput input);
    Task<VideoDto> UpdateAsync(Guid id, VideoDto input);
}