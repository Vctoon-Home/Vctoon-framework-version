using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Labs.Controls;
using CommunityToolkit.Mvvm.Input;
using VctoonClient.Navigations;
using VctoonClient.Oidc;
using VctoonClient.Stores.Users;

namespace VctoonClient.ViewModels;

public partial class MainViewModel : ViewModelBase, ITransientDependency
{
    [ObservableProperty]
    public bool _collapsed;


    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }


    public MainViewModel()
    {
        MenuItems = new ObservableCollection<MenuItemViewModel>()
        {
            new() {MenuHeader = "home", Key = "//home", MenuIconName = "mdi-home"},
            new() {MenuHeader = "login", Key = "//login", MenuIconName = "mdi-home"},
        };

        foreach (var menuItemViewModel in MenuItems)
        {
            SetActivateCommand(menuItemViewModel);
        }
    }

    // 递归设置所有menuItemViewModel.ActivateCommand
    private void SetActivateCommand(MenuItemViewModel menuItemViewModel)
    {
        menuItemViewModel.ActivateCommand = new RelayCommand(() => { NavigateTo(menuItemViewModel.Key); });
        foreach (var child in menuItemViewModel.Children)
        {
            SetActivateCommand(child);
        }
    }

    public async void NavigateTo(object key)
    {
        var navigationRouter = NavigationManager.Default.Router;

        await navigationRouter.NavigateToAsync(key);
    }
}