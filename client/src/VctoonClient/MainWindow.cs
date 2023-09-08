using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using VctoonClient.Views;

namespace VctoonClient;

public class MainWindow : Window, ISingletonDependency
{
    public MainWindow(MainView mainView)
    {
        Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://VctoonClient/Assets/avalonia-logo.ico")));
        Title = "VctoonClient";


        // var grid = new Grid()
        // {
        //     Children =
        //     {
        //         mainView
        //     }
        // };

        Content = mainView;

        #if DEBUG
                this.AttachDevTools();
        #endif
    }
}