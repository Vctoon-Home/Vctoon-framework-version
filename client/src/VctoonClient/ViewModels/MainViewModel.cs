using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public MainViewModel(UserStore userStore, ILoginService loginService)
    {
        _userStore = userStore;
        _loginService = loginService;
        _navigationRouter = new StackNavigationRouter();

        // _navigationRouter.NavigateToAsync(new NavigationViewModel(_navigationRouter));
        _navigationRouter.NavigateToAsync("//home");

        MenuItems = new ObservableCollection<MenuItemViewModel>()
        {
            new() {MenuHeader = "home", Key = "//home", MenuIconName = "mdi-home"},
            new() {MenuHeader = "login", Key = "//login", MenuIconName = "mdi-home"},
        };

        foreach (var menuItemViewModel in MenuItems)
        {
            SetActivateCommand(menuItemViewModel);
        }
    }

    // 递归设置所有menuItemViewModel.ActivateCommand
    private void SetActivateCommand(MenuItemViewModel menuItemViewModel)
    {
        menuItemViewModel.ActivateCommand = new RelayCommand( () =>
        {
            NavigateTo(menuItemViewModel.Key);
        });
        foreach (var child in menuItemViewModel.Children)
        {
            SetActivateCommand(child);
        }
    }

    public async void NavigateTo(object key)
    {
        if (NavigationRouter != null)
        {
            await NavigationRouter.NavigateToAsync(key);
        }
    }
}