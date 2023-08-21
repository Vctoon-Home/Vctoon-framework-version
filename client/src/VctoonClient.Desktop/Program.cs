using Avalonia;
using System;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using VctoonClient.Storages;
using VctoonClient.Storages.Base;

namespace VctoonClient.Desktop;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);


        AppStorageSavingHandler.SaveStorage();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var app = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .LogToTrace();

        return app;
    }
}