using Microsoft.AspNetCore.Mvc;

namespace VctoonCore.Resources;

public class TagAppService : CrudAppService<Tag, TagDto, Guid>, ITagAppService
{
    protected override string GetPolicyName { get; set; } = VctoonCorePermissions.Tag.Default;
    protected override string GetListPolicyName { get; set; } = VctoonCorePermissions.Tag.Default;
    protected override string CreatePolicyName { get; set; } = VctoonCorePermissions.Tag.Create;
    protected override string UpdatePolicyName { get; set; } = VctoonCorePermissions.Tag.Update;
    protected override string DeletePolicyName { get; set; } = VctoonCorePermissions.Tag.Delete;

    public TagAppService(ITagRepository repository) : base(repository)
    {
    }

    [HttpGet]
    [Route("api/app/get-all")]
    public async Task<List<TagDto>> GetAllAsync()
    {
        await CheckGetPolicyAsync();
        var tags = await Repository.GetListAsync();
        return ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
    }
}