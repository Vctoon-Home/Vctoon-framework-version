using System.Threading.Tasks;
using VctoonCore.Enums;
using Volo.Abp.DependencyInjection;

namespace VctoonCore.Libraries;

public interface ILibraryManager
{
    Task<Library> CreateAsync(string name, string[] paths, LibraryType libraryType);
    Task<Library> UpdateAsync(Library library);
    Task<Library> SetLibraryNameAsync(Guid id, string name);
    Task DeleteAsync(Library library);
}
