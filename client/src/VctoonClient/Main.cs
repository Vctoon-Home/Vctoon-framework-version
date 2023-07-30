using System;
using Avalonia.Controls;
using Avalonia.Platform;
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
    public MainView()
    {
        Content = App.Services.GetService<LoginView>();
    }
}
