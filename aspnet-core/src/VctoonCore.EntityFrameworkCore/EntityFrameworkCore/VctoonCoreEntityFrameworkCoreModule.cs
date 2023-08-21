using VctoonCore.Libraries;
using Microsoft.Extensions.DependencyInjection;
using VctoonCore.Resources;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace VctoonCore.EntityFrameworkCore;

[DependsOn(
    typeof(VctoonCoreDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
)]
public class VctoonCoreEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        VctoonCoreEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<VctoonCoreDbContext>(options => {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);

            // Libraries
            options.AddRepository<Library, LibraryRepository>();
            options.AddRepository<LibraryPath, LibraryPathRepository>();

            // Resources
            options.AddRepository<Comic, ComicRepository>();
            options.AddRepository<ComicChapter, ComicChapterRepository>();
            options.AddRepository<ComicImage, ComicImageRepository>();
            options.AddRepository<Tag, TagRepository>();
            options.AddRepository<Video, VideoRepository>();


        });

        Configure<AbpDbContextOptions>(options => {
            /* The main point to change your DBMS.
             * See also VctoonCoreMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql();
        });
    }
}
