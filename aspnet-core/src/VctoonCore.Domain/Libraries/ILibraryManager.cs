using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace VctoonCore.Libraries;

public interface ILibraryManager
{
    Task<Library> CreateAsync(string name, string[] paths);
    Task<Library> UpdateAsync(Library library);
    Task<Library> SetLibraryNameAsync(Guid id, string name);
    Task DeleteAsync(Library library);
}