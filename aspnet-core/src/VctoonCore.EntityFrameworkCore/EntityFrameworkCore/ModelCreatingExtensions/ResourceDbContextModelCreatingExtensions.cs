using VctoonCore.Libraries;
using VctoonCore.Resources;

namespace VctoonCore.EntityFrameworkCore.ModelCreatingExtensions;

public static class ResourceDbContextModelCreatingExtensions
{
    public static void ConfigureResources(this
        ModelBuilder builder)
    {
        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(VctoonCoreConsts.DbTablePrefix + "YourEntities", VctoonCoreConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        // builder.Owned<ComicArchiveInfo>();

        builder.Entity<Comic>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "Comics", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.HasMany(e => e.Tags).WithMany();
            b.HasMany(e => e.Chapters).WithOne().OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Video>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "Videos", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.HasMany(e => e.Tags).WithMany();
        });

        builder.Entity<Tag>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "Tags", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.HasIndex(e => e.Name).IsUnique();
        });

        builder.Entity<ComicChapter>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "ComicChapters", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasOne<LibraryPath>().WithMany().HasForeignKey(e => e.LibraryPathId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(e => e.Images).WithOne().HasForeignKey(e => e.ComicChapterId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ComicImage>(b =>
        {
            b.ToTable(VctoonCoreConsts.DbTablePrefix + "ComicImages", VctoonCoreConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });
    }
}