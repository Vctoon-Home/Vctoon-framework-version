using System.Linq;
using Abp.Localization.Avalonia;
using VctoonClient.Messages;
using VctoonClient.Navigations.Router;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonCore.Libraries;
using NavigationMenuItemProvider=VctoonClient.Navigations.Menus.NavigationMenuItemProvider;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ISingletonDependency
{
    private readonly ILoginService _loginService;
    private readonly UserStore _userStore;
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private NavigationMenuItemProvider _navigationMenuItemProvider;


    [ObservableProperty]
    private bool collapsed;

    [ObservableProperty]
    private IVctoonNavigationRouter router;

    public bool IsLogin => CurrentUser.IsAuthenticated;

    public string? UserName => CurrentUser?.UserName;
    public bool RouterCanGoBack => Router.CanGoBack;

    public MainViewModel(ILoginService loginService, LocalizationManager localizationManager, UserStore userStore,
        IVctoonNavigationRouter router, NavigationMenuItemProvider navigationMenuItemProvider,
        ILibraryAppService libraryAppService)
    {
        _loginService = loginService;
        _userStore = userStore;
        _navigationMenuItemProvider = navigationMenuItemProvider;
        _libraryAppService = libraryAppService;
        Router = router;

        MessengerRegister(localizationManager);
        Initialize();
    }

    public async void Initialize()
    {
        await Router.NavigateToAsync(NavigationMenuItemProvider.MenuItems.First().Path);
    }


    private void MessengerRegister(LocalizationManager localizationManager)
    {
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, async (r, m) => { UpdateProperties(); });
        WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) =>
        {
            NavigationMenuItemProvider.SetLibraryResources(null);

            UpdateProperties();
        });
        localizationManager.CurrentCultureChanged += (_, _) => { UpdateProperties(); };
        Router.Navigated += (_, _) => { UpdateProperties(); };
    }


    public async void Login()
    {
        await _loginService.LoginAsync();
    }

    public async void Logout()
    {
        await _loginService.LogoutAsync();
    }

    private void UpdateProperties()
    {
        OnPropertyChanged(nameof(IsLogin));
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(RouterCanGoBack));
    }
}