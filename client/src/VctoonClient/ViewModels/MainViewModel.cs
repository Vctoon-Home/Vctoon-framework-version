using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using VctoonClient.Navigations;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.ViewModels.Bases;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ITransientDependency
{
    private readonly UserStore _userStore;
    private readonly ILoginService _loginService;

    [ObservableProperty]
    private bool? _showNavBar = true;

    [ObservableProperty]
    private bool? _showBackButton = true;

    [ObservableProperty]
    private INavigationRouter _navigationRouter;

    [ObservableProperty]
    private bool _collapsed;

    public bool IsLogin => !string.IsNullOrEmpty(_userStore.AccessToken);

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public MainViewModel(UserStore userStore,ILoginService loginService)
    {
        _userStore = userStore;
        _loginService = loginService;
        _navigationRouter = new StackNavigationRouter();

        // _navigationRouter.NavigateToAsync(new NavigationViewModel(_navigationRouter));
        _navigationRouter.NavigateToAsync("//login");

        MenuItems = new ObservableCollection<MenuItemViewModel>()
        {
            new() {MenuHeader = "login", Key = "//login"},
        };
    }

    public async void NavigateTo(object page)
    {
        if (NavigationRouter != null)
        {
            await NavigationRouter.NavigateToAsync(page);
        }
    }
}