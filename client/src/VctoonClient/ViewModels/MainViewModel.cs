using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using Avalonia.Labs.Controls;
using Avalonia.Utilities;
using CommunityToolkit.Mvvm.Input;
using VctoonClient.Messages;
using VctoonClient.Navigations;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.Views;
using Volo.Abp.Users;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ISingletonDependency
{
    private readonly ILoginService _loginService;
    private readonly UserStore _userStore;

    [ObservableProperty]
    private  INavigationMenuItemProvider _navigationMenuItemProvider;

    [ObservableProperty]
    public bool _collapsed;

    // [ObservableProperty]
    // private bool _isLogin;

    [ObservableProperty]
    private IVctoonNavigationRouter _router;

    public bool IsLogin => CurrentUser.IsAuthenticated;

    public string UserName => CurrentUser.UserName;
    public bool RouterCanGoBack => Router.CanGoBack;


    public MainViewModel(ILoginService loginService, LocalizationManager localizationManager, UserStore userStore,
        IVctoonNavigationRouter router, INavigationMenuItemProvider navigationMenuItemProvider)
    {
        _loginService = loginService;
        _userStore = userStore;
        _navigationMenuItemProvider = navigationMenuItemProvider;
        Router = router;

        foreach (var menuItemViewModel in NavigationMenuItemProvider.MenuItems)
        {
            SetActivateCommand(menuItemViewModel);
        }

        _router.NavigateToAsync(NavigationMenuItemProvider.MenuItems.First().Path);

        MessengerRegister(localizationManager);
    }

    void MessengerRegister(LocalizationManager localizationManager)
    {
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) => { UpdateProperties(); });
        WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) => { UpdateProperties(); });
        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) => { UpdateProperties(); });
        localizationManager.PropertyChanged += (_, _) => { UpdateProperties(); };
    }


    public async void Login()
    {
        await _loginService.LoginAsync();
    }

    public async void Logout()
    {
        await _loginService.LogoutAsync();
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
        var paras = NavigationMenuItemProvider.MenuItems.First(m => m.Path == path).ClickNavigationParameters;

        await Router.NavigateToAsync(path, paras);
    }

    private void UpdateProperties()
    {
        OnPropertyChanged(nameof(IsLogin));
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(RouterCanGoBack));
    }


    public Task NavigationGoBack() => Router.BackAsync();
}