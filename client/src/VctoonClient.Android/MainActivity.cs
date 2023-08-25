using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using NativeAppStore;

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
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }

    protected override void OnPause()
    {
        StoreSaveExecutor.SaveAllStores();
        base.OnPause();
    }
}