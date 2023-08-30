using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abp.Localization.Avalonia;
using VctoonClient.Views;

namespace VctoonClient.Navigations.Menus;

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

        localizationManager.PropertyChanged += (sender, args) =>
        {
            MenuItems = GetMenuItems();
            NotifyMenuItemsChanged();
        };
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