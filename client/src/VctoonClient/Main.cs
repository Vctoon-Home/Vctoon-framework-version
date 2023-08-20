using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Platform;
using VctoonClient.Storages;
using VctoonClient.Storages.Base;
using VctoonClient.Views;

namespace VctoonClient;

public class MainWindow : Window, ISingletonDependency
{
    public MainWindow()
    {
        Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://VctoonClient/Assets/avalonia-logo.ico")));
        Title = "VctoonClient";
    }
}

public class MainView : UserControl, ISingletonDependency
{
    public MainView(IEnumerable<IAppStorage> storages)
    {
        Content = App.Services.GetService<LoginView>();

        this.Unloaded += (sender, args) =>
        {
            foreach (var storage in storages)
            {
                storage.SaveStorage();
            }
        };
        
        // save settings on close
        // this.exit += (sender, args) =>
        // {
        //     foreach (var storage in storages)
        //     {
        //         storage.SaveStorage();
        //     }
        // };
    }
}