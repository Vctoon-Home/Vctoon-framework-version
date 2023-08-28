using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using Avalonia.Labs.Controls;
using Avalonia.Utilities;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    public bool _collapsed;

    // [ObservableProperty]
    // private bool _isLogin;

    [ObservableProperty]
    private INavigationRouter _navigationRouter = NavigationProvider.Default.Router;

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public bool IsLogin => CurrentUser.IsAuthenticated;


    public string UserName => CurrentUser.UserName;


    public MainViewModel(ILoginService loginService, LocalizationManager localizationManager, UserStore userStore)
    {
        _loginService = loginService;
        _userStore = userStore;

        MenuItems = new()
        {
            new()
            {
                MenuHeader = localizationManager["Home"], Path = "//home", MenuIconName = "mdi-home",
                ViewType = typeof(HomeView)
            },
        };

        foreach (var menuItemViewModel in MenuItems)
        {
            SetActivateCommand(menuItemViewModel);
        }

        _navigationRouter.NavigateToAsync(MenuItems.First().Path);

        MessengerRegister(localizationManager);

    }

    void MessengerRegister(LocalizationManager localizationManager)
    {
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) => { UpdateProperties(); });

        WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) => { UpdateProperties(); });

        localizationManager.PropertyChanged += (_, _) => { UpdateProperties(); };
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

    private void UpdateProperties()
    {
        OnPropertyChanged(nameof(IsLogin));
        OnPropertyChanged(nameof(UserName));

        var user = CurrentUser;
        var user1 = App.Services.GetService<ICurrentUser>();
    }
}