using VctoonCore.EntityFrameworkCore;

namespace VctoonCore;

[DependsOn(
    typeof(VctoonCoreEntityFrameworkCoreTestModule)
)]
public class VctoonCoreDomainTestModule : AbpModule
{

}