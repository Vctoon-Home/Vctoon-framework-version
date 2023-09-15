using Avalonia.Controls;
using Avalonia.Interactivity;
using VctoonClient.ViewModels.Homes;

namespace VctoonClient.Views.Homes;

public partial class HomeView : UserControl, ITransientDependency
{
    private readonly HomeViewModel _vm;

    public HomeView()
    {
        _vm = App.Services.GetRequiredService<HomeViewModel>();
        InitializeComponent();

        DataContext = _vm;
    }


    private void Notification_Click(object? sender, RoutedEventArgs e)
    {
        App.NotificationManager?.Show(new Notification("666", "This is message", NotificationType.Error));
    }

}