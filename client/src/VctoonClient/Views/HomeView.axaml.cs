using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VctoonClient.Navigations;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class HomeView : UserControl, ITransientDependency
{
    private readonly HomeViewModel _vm;

    public HomeView()
    {
        _vm = App.Services.GetRequiredService<HomeViewModel>();
        InitializeComponent();

        DataContext = _vm;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
    }
}