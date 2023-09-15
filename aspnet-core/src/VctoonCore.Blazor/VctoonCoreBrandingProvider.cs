using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace VctoonCore.Blazor;

[Dependency(ReplaceServices = true)]
public class VctoonCoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "VctoonCore";
}