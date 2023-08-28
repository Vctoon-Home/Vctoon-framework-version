using Avalonia.Labs.Controls;
using VctoonClient.Navigations;

namespace VctoonClient.Layouts.Main;

public partial class MainContentViewModel : ViewModelBase, ITransientDependency
{
    [ObservableProperty]
    private INavigationRouter _navigationRouter = NavigationManager.Default.Router;

    public MainContentViewModel()
    {
        _navigationRouter.NavigateToAsync("//home");
    }
}