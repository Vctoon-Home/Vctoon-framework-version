using System.Threading.Tasks;
using VctoonCore.Permissions;
using VctoonCore.Localization;
using VctoonCore.MultiTenancy;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace VctoonCore.Blazor.Menus;

public class VctoonCoreMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<VctoonCoreResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                VctoonCoreMenus.Home,
                l["Menu:Home"],
                "/",
                "fas fa-home",
                0
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        if (await context.IsGrantedAsync(VctoonCorePermissions.Library.Default))
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(VctoonCoreMenus.Library, l["Menu:Library"], "/Libraries/Library")
            );
        }
    }
}