using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using VctoonClient.Stores.Users;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private readonly MainViewModel _vm;



    public MainView()
    {
        InitializeComponent();

        _vm = App.Services.GetService<MainViewModel>();
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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel != null)
        {
            topLevel.BackRequested += TopLevel_BackRequested;
        }
    }

    private async void TopLevel_BackRequested(object? sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as MainViewModel;

        if (viewModel != null)
        {
            if (viewModel.NavigationRouter.CanGoBack)
            {
                e.Handled = true;

                await viewModel.NavigationRouter.BackAsync();
            }
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel != null)
        {
            topLevel.BackRequested -= TopLevel_BackRequested;
        }
    }
}