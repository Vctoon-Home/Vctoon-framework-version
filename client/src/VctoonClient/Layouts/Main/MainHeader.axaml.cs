using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using VctoonClient.Oidc;
using VctoonClient.Stores.Apps;

namespace VctoonClient.Layouts.Main;

public partial class MainHeader : UserControl
{
    private MainHeaderViewModel _vm;

    public MainHeader()
    {
        InitializeComponent();

        _vm = App.Services.GetService<MainHeaderViewModel>()!;
        this.DataContext = _vm;
    }

    private void ToggleButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
    {
        var app = Application.Current;
        if (app is not null)
        {
            var theme = app.ActualThemeVariant;
            app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        }
    }
}