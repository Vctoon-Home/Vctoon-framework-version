namespace VctoonCore;

[DependsOn(
    typeof(VctoonCoreApplicationModule),
    typeof(VctoonCoreDomainTestModule)
)]
public class VctoonCoreApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => { options.IsJobExecutionEnabled = true; });
    }
}