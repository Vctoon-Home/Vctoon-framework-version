using System;
using System.Reflection;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using VctoonClient.Views;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace VctoonClient;

public partial class App : Application
{
    private static IServiceCollection ServiceCollection;

    public static IServiceProvider Services;

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
        Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>()
            .Initialize(Services);

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = Services.GetRequiredService<MainView>();
        }


        base.OnFrameworkInitializationCompleted();
    }
}