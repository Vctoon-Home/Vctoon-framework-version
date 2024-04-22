using Shouldly;
using System.Threading.Tasks;
using VctoonCore.Libraries.Dtos;
using VctoonCore.TestDatas;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace VctoonCore.Libraries;

public class LibraryAppServiceTests : VctoonCoreApplicationTestBase
{
    // private readonly ILibraryRepository _libraryRepository;

    public LibraryAppServiceTests()
    {

    }

    [Fact]
    public async Task Should_Get_List_Of_Libraries()
    {

        var _libraryRepository = GetRequiredService<ILibraryRepository>();


        var libs = await _libraryRepository.ToListAsync();


        // Arrange
        var libraryAppService = GetRequiredService<ILibraryAppService>();

        // Act
        var result = await libraryAppService.GetListAsync(new LibraryGetListInput());

        // Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(l => l.Name == LibraryTestData.Name);
    }
}