using System.Threading.Tasks;
using VctoonCore.Libraries;
using VctoonCore.TestDatas;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace VctoonCore;

public class VctoonCoreTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ILibraryRepository _libraryRepository;

    public VctoonCoreTestDataSeedContributor(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        var library = new Library(LibraryTestData.Id, LibraryTestData.Name);
        library.Paths.AddRange(LibraryTestData.Paths);

        await _libraryRepository.InsertAsync(library);
    }
}