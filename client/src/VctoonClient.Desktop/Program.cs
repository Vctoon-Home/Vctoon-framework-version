using Avalonia;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using NativeAppStore;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using Projektanker.Icons.Avalonia.MaterialDesign;
using VctoonClient.Desktop.Oidc;
using VctoonClient.Oidc;

namespace VctoonClient.Desktop;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        if (args.Any())
        {
            await ProcessCallback(args[0]);
        }
        else
        {
            new RegistryConfig("VctoonCore").Configure();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);

            StoreSaveExecutor.SaveAllStores();
        }
    }

    private static async Task ProcessCallback(string args)
    {
        var response = new AuthorizeResponse(args);
        if (!String.IsNullOrWhiteSpace(response.State))
        {
            Console.WriteLine($"Found state: {response.State}");
            var callbackManager = new CallbackManager(response.State);
            await callbackManager.RunClient(args);
        }
        else
        {
            Console.WriteLine("Error: no state on response");
        }
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