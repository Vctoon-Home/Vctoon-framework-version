using System.Threading.Tasks;

namespace VctoonCore.Libraries;

/// <summary>
/// 
/// </summary>
public interface ILibraryRepository : IRepository<Library, Guid>
{
    Task<Library> FindByNameAsync(string name);
}