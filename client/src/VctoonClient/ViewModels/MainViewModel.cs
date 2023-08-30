using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using CommunityToolkit.Mvvm.Input;
using VctoonClient.Messages;
using VctoonClient.Navigations;
using VctoonClient.Navigations.Menus;
using VctoonClient.Navigations.Router;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;
using VctoonClient.Views;
using VctoonCore.Enums;
using VctoonCore.Libraries;
using NavigationMenuItemProvider = VctoonClient.Navigations.Menus.NavigationMenuItemProvider;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ISingletonDependency
{
    private readonly ILoginService _loginService;
    private readonly UserStore _userStore;

    [ObservableProperty]
    private NavigationMenuItemProvider _navigationMenuItemProvider;

    private readonly ILibraryAppService _libraryAppService;

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
        foreach (var menuItemViewModel in NavigationMenuItemProvider.MenuItems)
            SetActivateCommand(menuItemViewModel);

        await Router.NavigateToAsync(NavigationMenuItemProvider.MenuItems.First().Path);
    }


    public async Task InitializeWhenViewIsLoaded()
    {
        if (IsLogin)
        {
            NavigationMenuItemProvider.SetLibraryResources(await GetLibraryResources());
        }
    }


    void MessengerRegister(LocalizationManager localizationManager)
    {
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, async (r, m) =>
        {
            NavigationMenuItemProvider.SetLibraryResources(await GetLibraryResources());
            UpdateProperties();
        });
        WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) =>
        {
            NavigationMenuItemProvider.SetLibraryResources(null);

            UpdateProperties();
        });
        localizationManager.PropertyChanged += (_, _) =>
        {
            UpdateProperties();
        };
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

    // 递归设置所有menuItemViewModel.ActivateCommand
    private void SetActivateCommand(MenuItemViewModel item)
    {
        item.ActivateCommand = new AsyncRelayCommand(async () =>
        {
            await Router.NavigateToAsync(item.Path, item.ClickNavigationParameters);
        });
        foreach (var child in item.Children)
        {
            SetActivateCommand(child);
        }
    }


    public async Task<ObservableCollection<MenuItemViewModel>> GetLibraryResources()
    {
        var libraries = await _libraryAppService.GetLibraryMenuAsync();

        var menus = libraries.Select(l => new MenuItemViewModel()
        {
            IsResource = true,
            Header = l.Name,
            Icon = l.LibraryType == LibraryType.Comic ? "mdi-bookshelf" : "mdi-movie-filter",
            Path = $"//library/{l.Id}",
            ClickNavigationParameters = new Dictionary<string, object>()
            {
                {"LibraryId", l.Id},
            },
            ViewType = typeof(LibraryView)
        }).ToList();

        foreach (var menuItemViewModel in menus)
        {
            SetActivateCommand(menuItemViewModel);
        }

        return new ObservableCollection<MenuItemViewModel>(menus);
    }


    private void UpdateProperties()
    {
        OnPropertyChanged(nameof(IsLogin));
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(RouterCanGoBack));
    }


    public Task NavigationGoBack() => Router.BackAsync();
}