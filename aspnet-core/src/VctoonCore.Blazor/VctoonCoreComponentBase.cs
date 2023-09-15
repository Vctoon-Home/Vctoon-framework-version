using VctoonCore.Localization;
using Volo.Abp.AspNetCore.Components;

namespace VctoonCore.Blazor;

public abstract class VctoonCoreComponentBase : AbpComponentBase
{
    protected VctoonCoreComponentBase()
    {
        LocalizationResource = typeof(VctoonCoreResource);
    }
}