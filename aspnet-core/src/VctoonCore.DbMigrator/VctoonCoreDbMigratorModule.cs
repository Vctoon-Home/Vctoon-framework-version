using VctoonCore.EntityFrameworkCore;

namespace VctoonCore.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VctoonCoreEntityFrameworkCoreModule),
    typeof(VctoonCoreApplicationContractsModule)
)]
public class VctoonCoreDbMigratorModule : AbpModule
{

}