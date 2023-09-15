using System;
using VctoonCore.Resources.DataResolves;

namespace VctoonCore.ArchiveDataHandlers;

public class ArchiveDataResolveTests : VctoonCoreDomainTestBase
{
    [Fact]
    public void Should_Resolve_ArchiveData()
    {
        var guidGenerator = GetRequiredService<IGuidGenerator>();

        // Arrange
        var archiveDataResolve = new ArchiveDataResolve(@$"E:\Downloads\gmzr\image (2).zip",
            ResourceSupportFileExtensions.ComicImageExtensions);

        // Act
        var comics = archiveDataResolve.GetComicChapters(guidGenerator, Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.NotNull(comics);
    }
}