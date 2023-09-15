using Volo.Abp.Application.Services;

namespace VctoonCore.Resources;

public interface ITagAppService : ICrudAppService<
    TagDto,
    Guid>
{
    Task<List<TagDto>> GetAllAsync();
    Task Deletes(Guid[] ids);
}