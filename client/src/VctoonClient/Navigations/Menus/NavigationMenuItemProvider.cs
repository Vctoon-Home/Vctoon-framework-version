using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using VctoonClient.Messages;
using VctoonClient.Views;
using VctoonCore.Enums;
using VctoonCore.Libraries;
using HomeView = VctoonClient.Views.Homes.HomeView;
using LibraryView = VctoonClient.Views.Libraries.LibraryView;

namespace VctoonClient.Navigations.Menus;

public partial class NavigationMenuItemProvider : ObservableObject, ISingletonDependency
{
    private readonly ILocalizationManager _localizationManager;
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _menuItems;

    private MenuItemViewModel RootResourceItem;

    public NavigationMenuItemProvider(ILocalizationManager localizationManager, ILibraryAppService libraryAppService)
    {
        _localizationManager = localizationManager;
        _libraryAppService = libraryAppService;
        MenuItems = GetMenuItems();

        localizationManager.CurrentCultureChanged += (sender, args) =>
        {
            MenuItems = GetMenuItems();
            NotifyMenuItemsChanged();
        };

        WeakReferenceMessenger.Default.Register<LoginMessage>(this, (_, _) => { MenuItems = GetMenuItems(); });
    }

    private ObservableCollection<MenuItemViewModel> GetMenuItems()
    {
        RootResourceItem = new()
        {
            Header = _localizationManager["Menu:Libraries"], Icon = "fa-landmark-flag",
            IsRootResource = true,
        };


        var items = new ObservableCollection<MenuItemViewModel>()
        {
            new()
            {
                Header = _localizationManager["Menu:Home"], Path = "//home", Icon = "mdi-home",
                ViewType = typeof(HomeView)
            },
            RootResourceItem
        };


        foreach (var menuItemViewModel in items)
        {
            SetActivateCommand(menuItemViewModel);
        }

        Task.Run(async () =>
        {
            var data = await GetLibraryResources();

            foreach (var menuItemViewModel in data)
            {
                SetActivateCommand(menuItemViewModel);
            }

            RootResourceItem.Children = data;
            Dispatcher.UIThread.Invoke(NotifyMenuItemsChanged);
        });


        return items;
    }

    // 递归设置所有menuItemViewModel.ActivateCommand
    private void SetActivateCommand(MenuItemViewModel item)
    {
        item.ActivateCommand = new AsyncRelayCommand(async () =>
        {
            await App.Router.NavigateToAsync(item.Path, item.ClickNavigationParameters);
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


        return new ObservableCollection<MenuItemViewModel>(menus);
    }


    public void SetLibraryResources(ObservableCollection<MenuItemViewModel>? resources)
    {
        RootResourceItem.Children = resources ?? new ObservableCollection<MenuItemViewModel>();
        NotifyMenuItemsChanged();
    }

    public MenuItemViewModel? GetMenuItemByPath(string path)
    {
        return GetMenuItem(path, MenuItems);
    }

    public void NotifyMenuItemsChanged()
    {
        var oldRes = MenuItems;
        MenuItems = null;
        MenuItems = oldRes;
    }


    private MenuItemViewModel? GetMenuItem(string path, ObservableCollection<MenuItemViewModel> items)
    {
        if (items.IsNullOrEmpty())
            return null;

        foreach (var item in items)
        {
            if (item.Path == path)
                return item;

            var result = GetMenuItem(path, item.Children);
            if (result != null)
                return result;
        }

        return null;
    }
}