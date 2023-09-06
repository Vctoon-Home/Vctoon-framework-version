using Volo.Abp;

namespace VctoonCore.Systems;

public interface ISystemAppService : IRemoteService
{
    Task<List<SystemFolderDto>> GetSystemFolder(string? path = null);
}