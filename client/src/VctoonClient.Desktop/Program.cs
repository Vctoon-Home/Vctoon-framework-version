using Avalonia;
using System;
using System.Threading.Tasks;
using NativeAppStore;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using Projektanker.Icons.Avalonia.MaterialDesign;
using VctoonClient.Desktop.Oidc;

namespace VctoonClient.Desktop;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        new RegistryConfig("VctoonCore").Configure();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        StoreSaveExecutor.SaveAllStores();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>()
            .Register<MaterialDesignIconProvider>();

        var app = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

        return app;
    }
}