using VctoonCore.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace VctoonCore;

[DependsOn(
    typeof(VctoonCoreEntityFrameworkCoreTestModule)
    )]
public class VctoonCoreDomainTestModule : AbpModule
{

}
