using Avalonia.Controls;
using Avalonia.Interactivity;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private readonly MainViewModel _vm;

    public MainView(MainViewModel vm)
    {
        InitializeComponent();
        
        _vm = vm;
        this.DataContext = vm;
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