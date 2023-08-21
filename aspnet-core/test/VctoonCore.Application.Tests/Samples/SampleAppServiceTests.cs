using System.IO;
using Shouldly;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using Volo.Abp.Identity;
using Xunit;

namespace VctoonCore.Samples;

/* This is just an example test class.
 * Normally, you don't test code of the modules you are using
 * (like IIdentityUserAppService here).
 * Only test your own application services.
 */
public class SampleAppServiceTests : VctoonCoreApplicationTestBase
{
    private readonly IIdentityUserAppService _userAppService;

    public SampleAppServiceTests()
    {
        _userAppService = GetRequiredService<IIdentityUserAppService>();
    }

    [Fact]
    public async Task Initial_Data_Should_Contain_Admin_User()
    {
        //Act
        var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(u => u.UserName == "admin");
    }

    [Fact]
    public async Task Archive_Test()
    {
        await using Stream stream = File.OpenRead(@$"E:\Downloads\gmzr\ar\ar.7z");
        IArchive zip = ZipArchive.Open(stream);

        // while (reader.MoveToNextEntry())
        // {
        //     if (!reader.Entry.IsDirectory)
        //     {
        //         // Console.WriteLine(reader.Entry.Key);
        //         // reader.WriteEntryToDirectory(@"C:\temp",
        //         //     new ExtractionOptions() {ExtractFullPath = true, Overwrite = true});
        //     }
        //     else
        //     {
        //     }
        // }
    }
}