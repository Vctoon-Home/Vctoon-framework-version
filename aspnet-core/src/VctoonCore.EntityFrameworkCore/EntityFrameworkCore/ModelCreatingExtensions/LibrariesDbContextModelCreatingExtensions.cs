using VctoonCore.Enums;
using VctoonCore.Libraries;
using VctoonCore.Resources;

namespace VctoonCore.EntityFrameworkCore.ModelCreatingExtensions;

public static class LibrariesDbContextModelCreatingExtensions
{
    public static void ConfigureLibraries(this
        ModelBuilder builder)
    {
        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(VctoonCoreConsts.DbTablePrefix + "YourEntities", VctoonCoreConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Library>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "Libraries", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention();//auto configure for the base class props

            b.HasIndex(e => e.Name).IsUnique();
            // b.HasMany(e => e.Paths).WithOne(e => e.Library).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.Paths).WithOne().OnDelete(DeleteBehavior.Cascade);
            b.HasMany<Video>().WithOne().HasForeignKey(e => e.LibraryId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany<Comic>().WithOne().HasForeignKey(e => e.LibraryId).OnDelete(DeleteBehavior.Cascade);

            b.Property(e => e.LibraryType).HasConversion(v => v.ToString(), v => (LibraryType) Enum.Parse(typeof(LibraryType), v));

        });

        builder.Entity<LibraryPath>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "LibraryPaths", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention();//auto configure for the base class props

            b.HasIndex(e => e.Path).IsUnique();
            b.HasMany(e => e.Children).WithOne().HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.Files).WithOne().HasForeignKey(e => e.LibraryPathId).OnDelete(DeleteBehavior.Cascade);
        });


        // LibraryFile
        builder.Entity<LibraryFile>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "LibraryFiles", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention();//auto configure for the base class props
        });
    }
}