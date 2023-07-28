using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Microsoft.Extensions.Configuration;
using VctoonClient.Localizations;
using VctoonClient.ViewModels;
using Volo.Abp.DependencyInjection;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private MainViewModel _vm;

    public MainView(MainViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
        _vm = vm;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine(123);
        var manager = App.ServiceProvider.GetService<LocalizationManager>();

        var currentCulture = manager.CurrentCulture;

        if (currentCulture.Name == "en")
        {
            currentCulture = new CultureInfo("zh-Hans");
        }
        else
        {
            currentCulture = new CultureInfo("en");
        }

        manager.CurrentCulture = currentCulture;


        // if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        // {
        //     desktop.MainWindow.Content = App.ServiceProvider.GetService<LoginView>();
        // }
        // else if (Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        // {
        //     singleViewPlatform.MainView = App.ServiceProvider.GetService<LoginView>();
        // }
    }
}