using Volo.Abp;

namespace VctoonCore.Resources;

public class VideoAppService : CrudAppService<Video, VideoDto, Guid, GetVideosInput>
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.Video.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.Video.Default;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.Video.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.Video.Delete;


    public VideoAppService(IVideoRepository repository) : base(repository)
    {
    }

    public async Task DeleteAsync(Guid id, bool deleteInRealFileSystem = false)
    {
        await base.DeleteAsync(id);

        // TODO: Delete in real file system
        if (deleteInRealFileSystem)
        {
        }
    }


    [Obsolete]
    [RemoteService(false)]
    public override Task<VideoDto> CreateAsync(VideoDto input)
    {
        return base.CreateAsync(input);
    }

    [Obsolete]
    [RemoteService(false)]
    public override Task DeleteAsync(Guid id)
    {
        return base.DeleteAsync(id);
    }
}