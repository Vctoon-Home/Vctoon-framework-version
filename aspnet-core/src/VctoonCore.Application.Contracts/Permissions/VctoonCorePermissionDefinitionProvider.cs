using VctoonCore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace VctoonCore.Permissions;

public class VctoonCorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(VctoonCorePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(VctoonCorePermissions.MyPermission1, L("Permission:MyPermission1"));

        var libraryPermission = myGroup.AddPermission(VctoonCorePermissions.Library.Default, L("Permission:Library"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Create, L("Permission:Create"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Update, L("Permission:Update"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VctoonCoreResource>(name);
    }
}
