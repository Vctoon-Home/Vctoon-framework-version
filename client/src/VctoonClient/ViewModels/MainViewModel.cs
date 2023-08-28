using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.Input;
using VctoonClient.Navigations;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.Views;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ISingletonDependency
{
    private readonly ILoginService _loginService;

    [ObservableProperty]
    public bool _collapsed;

    [ObservableProperty]
    private bool _isLogin;

    [ObservableProperty]
    private INavigationRouter _navigationRouter = NavigationProvider.Default.Router;

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }


    public MainViewModel(ILoginService loginService)
    {
        _loginService = loginService;

        MenuItems = new()
        {
            new() {MenuHeader = L["Home"], Path = "//home", MenuIconName = "mdi-home", ViewType = typeof(HomeView)},
        };

        foreach (var menuItemViewModel in MenuItems)
        {
            SetActivateCommand(menuItemViewModel);
        }

        _navigationRouter.NavigateToAsync(MenuItems.First().Path);
    }

    public async void Login()
    {
        await _loginService.LoginAsync();
    }

    // 递归设置所有menuItemViewModel.ActivateCommand
    private void SetActivateCommand(MenuItemViewModel menuItemViewModel)
    {
        menuItemViewModel.ActivateCommand = new RelayCommand(() => { NavigateTo(menuItemViewModel.Path); });
        foreach (var child in menuItemViewModel.Children)
        {
            SetActivateCommand(child);
        }
    }

    private async void NavigateTo(string path)
    {
        var navigationRouter = NavigationProvider.Default.Router;

        var paras = MenuItems.First(m => m.Path == path).ClickNavigationParameters;

        await navigationRouter.NavigateToAsync(path, paras);
    }
}