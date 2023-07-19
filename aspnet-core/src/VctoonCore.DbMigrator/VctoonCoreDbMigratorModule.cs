using VctoonCore.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace VctoonCore.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VctoonCoreEntityFrameworkCoreModule),
    typeof(VctoonCoreApplicationContractsModule)
    )]
public class VctoonCoreDbMigratorModule : AbpModule
{

}
