using Volo.Abp.Settings;

namespace VctoonCore.Settings;

public class VctoonCoreSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(VctoonCoreSettings.MySetting1));
    }
}
