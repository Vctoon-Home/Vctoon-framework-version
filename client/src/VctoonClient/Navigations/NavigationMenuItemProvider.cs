using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Abp.Localization.Avalonia;
using Avalonia.Threading;
using Ursa.Controls;
using VctoonClient.Views;
using VctoonCore.Libraries;

namespace VctoonClient.Navigations;

public partial class NavigationMenuItemProvider : ObservableObject, ISingletonDependency
{
    private readonly LocalizationManager _localizationManager;

    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _menuItems;

    private MenuItemViewModel RootResourceItem;

    public NavigationMenuItemProvider(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        MenuItems = GetMenuItems();
    }

    private ObservableCollection<MenuItemViewModel> GetMenuItems()
    {
        RootResourceItem = new()
        {
            Header = _localizationManager["Menu:Libraries"], Icon = "fa-landmark-flag",
            IsRootResource = true
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

        return items;
    }


    public void SetLibraryResources(ObservableCollection<MenuItemViewModel>? resources)
    {
        RootResourceItem.Children = resources ?? new ObservableCollection<MenuItemViewModel>();

        var oldRes = MenuItems;
        MenuItems = null;
        MenuItems = oldRes;
    }

    public MenuItemViewModel? GetMenuItemByPath(string path)
    {
        return GetMenuItem(path, MenuItems);
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