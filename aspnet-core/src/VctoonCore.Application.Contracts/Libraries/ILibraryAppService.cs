using VctoonCore.Libraries.Dtos;
using Volo.Abp.Application.Services;

namespace VctoonCore.Libraries;


/// <summary>
/// 
/// </summary>
public interface ILibraryAppService :
    ICrudAppService< 
        LibraryDto, 
        Guid, 
        LibraryGetListInput,
        LibraryCreateUpdateDto,
        LibraryCreateUpdateDto>
{
    Task<List<LibraryDto>> GetLibraryMenuAsync();
}