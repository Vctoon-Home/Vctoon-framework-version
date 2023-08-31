using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class HomeView : UserControl, ITransientDependency
{
    private readonly HomeViewModel _vm;
    private WindowNotificationManager? _manager;

    public HomeView()
    {
        _vm = App.Services.GetRequiredService<HomeViewModel>();
        InitializeComponent();

        DataContext = _vm;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var topLevel = TopLevel.GetTopLevel(this);
        _manager = new WindowNotificationManager(topLevel) {MaxItems = 3};
    }

    private void Notification_Click(object? sender, RoutedEventArgs e)
    {
        _manager?.Show(new Notification("666", "This is message", NotificationType.Error));
    }
}