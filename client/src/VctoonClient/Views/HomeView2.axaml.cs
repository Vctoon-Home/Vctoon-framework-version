using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VctoonClient.Navigations;

namespace VctoonClient.Views;

public partial class HomeView2 : UserControl
{
    public HomeView2()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        App.Services.GetService<IVctoonNavigationRouter>()!.NavigateToAsync(new HomeView2());
    }
}