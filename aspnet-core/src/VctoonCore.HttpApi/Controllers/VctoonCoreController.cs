using VctoonCore.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace VctoonCore.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class VctoonCoreController : AbpControllerBase
{
    protected VctoonCoreController()
    {
        LocalizationResource = typeof(VctoonCoreResource);
    }
}
