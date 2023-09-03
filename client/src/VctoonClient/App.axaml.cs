using System;
using System.Reflection;
using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using VctoonClient.Navigations.Router;
using VctoonClient.Validations;
using VctoonClient.Views;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace VctoonClient;

public partial class App : Application
{
    private static IServiceCollection ServiceCollection;

    public static IServiceProvider Services;

    public static IVctoonNavigationRouter Router => Services.GetRequiredService<IVctoonNavigationRouter>();


    public static WindowNotificationManager NotificationManager { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();

        ServiceCollection = new ServiceCollection();

        var configureConfiguration = new ConfigurationManager();
        ConfigureConfiguration(configureConfiguration);

        ServiceCollection.AddApplication<VctoonClientModule>(options =>
        {
            options.Services.ReplaceConfiguration(configureConfiguration);
        });
        var factory = new AbpAutofacServiceProviderFactory(new Autofac.ContainerBuilder());

        var builder = factory.CreateBuilder(ServiceCollection);

        builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

        Services = factory.CreateServiceProvider(builder);
    }


    private static void ConfigureConfiguration(IConfigurationBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder
            .AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        BindingPlugins.DataValidators.RemoveAt(0);
        BindingPlugins.DataValidators.Add(new VctoonClientDataAnnotationsValidationPlugin());
        
        Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>()
            .Initialize(Services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();

            SetWindowNotificationManager((UserControl) desktop.MainWindow!.Content!);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = Services.GetRequiredService<MainView>();
            SetWindowNotificationManager((UserControl) singleViewPlatform.MainView);
        }

        base.OnFrameworkInitializationCompleted();
    }

    void SetWindowNotificationManager(UserControl topView)
    {
        topView.AttachedToVisualTree += (_, _) =>
        {
            var topLevel = TopLevel.GetTopLevel(topView);
            NotificationManager = new WindowNotificationManager(topLevel)
            {
                MaxItems = 5
            };
        };
    }
}