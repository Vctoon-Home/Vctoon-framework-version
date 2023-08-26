using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using NativeAppStore;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using Projektanker.Icons.Avalonia.MaterialDesign;

namespace VctoonClient.Android;

[Activity(
    Label = "VctoonClient.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>()
            .Register<MaterialDesignIconProvider>();

        var app = base.CustomizeAppBuilder(builder)
            .WithInterFont();

        return app;
    }

    protected override void OnPause()
    {
        StoreSaveExecutor.SaveAllStores();
        base.OnPause();
    }
}