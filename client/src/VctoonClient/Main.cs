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
    public MainWindow(IEnumerable<IAppStorage> storages)
    {
        Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://VctoonClient/Assets/avalonia-logo.ico")));
        Title = "VctoonClient";

        // save settings on close
        this.Closing += (sender, args) =>
        {
            foreach (var storage in storages)
            {
                storage.Save();
            }
        };
    }
}

public class MainView : UserControl, ISingletonDependency
{
    public MainView()
    {
        Content = App.Services.GetService<LoginView>();
    }
}