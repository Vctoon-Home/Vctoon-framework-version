using System.Collections.ObjectModel;
using Abp.Localization.Avalonia;
using Ursa.Controls;
using VctoonClient.Views;

namespace VctoonClient.Navigations;

[INotifyPropertyChanged]
public partial class VctoonNavigationMenuItemProvider : INavigationMenuItemProvider, ISingletonDependency
{
    private readonly LocalizationManager _localizationManager;

    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _menuItems;


    public VctoonNavigationMenuItemProvider(LocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        MenuItems = GetMenuItems();
    }

    public ObservableCollection<MenuItemViewModel> GetMenuItems()
    {
        return new ObservableCollection<MenuItemViewModel>()
        {
            new()
            {
                MenuHeader = _localizationManager["Home"], Path = "//home", MenuIconName = "mdi-home",
                ViewType = typeof(HomeView)
            },
            new()
            {
                MenuHeader = "Home1", Path = "//home1", MenuIconName = "mdi-test",
                ViewType = typeof(HomeView1)
            },
        };
    }
}