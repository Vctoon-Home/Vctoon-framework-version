using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using VctoonClient.ViewModels;
using VctoonClient.Views;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace VctoonClient;

public partial class App : Application
{
    public static IServiceCollection ServiceCollection;

    public static IServiceProvider ServiceProvider;

    public App()
    {
        CreateServices();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void CreateServices()
    {
        ServiceCollection = new ServiceCollection();

        var configureConfiguration = new ConfigurationManager();
        ConfigureConfiguration(configureConfiguration);

        ServiceCollection.AddApplication<VctoonClientModule>(options =>
        {
            options.Services.ReplaceConfiguration(configureConfiguration);
        });
        var factory = new AbpAutofacServiceProviderFactory(new Autofac.ContainerBuilder());

        ServiceProvider = factory.CreateServiceProvider(factory.CreateBuilder(ServiceCollection));
    }

    private static void ConfigureConfiguration(IConfigurationBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder
            .AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceProvider.GetRequiredService<IAbpApplicationWithExternalServiceProvider>()
            .Initialize(ServiceProvider);


        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = ServiceProvider.GetRequiredService<MainView>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}