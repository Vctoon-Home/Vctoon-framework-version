using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using VctoonClient.ViewModels.Bases;
using VctoonClient.Views;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ITransientDependency
{
    [ObservableProperty]
    private bool? _showNavBar = true;

    [ObservableProperty]
    private bool? _showBackButton = true;

    [ObservableProperty]
    private INavigationRouter _navigationRouter;

    public MainViewModel()
    {
        _navigationRouter = new StackNavigationRouter();

        // _navigationRouter.NavigateToAsync(new NavigationViewModel(_navigationRouter));
        _navigationRouter.NavigateToAsync("//login");
    }

    static MainViewModel()
    {
        ViewLocator.Register(typeof(MainViewModel), () => App.Services.GetService<MainView>());
    }

    public async void NavigateTo(object page)
    {
        if (NavigationRouter != null)
        {
            await NavigationRouter.NavigateToAsync(page);
        }
    }
}