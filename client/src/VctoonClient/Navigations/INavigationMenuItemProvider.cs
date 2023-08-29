using System.Collections.ObjectModel;
using Ursa.Controls;

namespace VctoonClient.Navigations;

public interface INavigationMenuItemProvider
{
    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
}