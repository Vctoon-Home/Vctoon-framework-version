using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VctoonClient.Navigations.Router;
using VctoonClient.ViewModels;
using VctoonClient.Views.Libraries;

namespace VctoonClient.Layouts.Main;

public partial class MainNavigationBar : UserControl
{
    private readonly MainViewModel _vm;

    public MainNavigationBar()
    {
        InitializeComponent();

        _vm = App.Services.GetRequiredService<MainViewModel>();
        this.DataContext = _vm;


        SetRootBorderHasPadding(!Menu.IsClosed);
        Menu.PropertyChanged += (sender, args) =>
        {
            if (args.Property.Name == nameof(Menu.IsClosed))
            {
                SetRootBorderHasPadding(args.NewValue is false);
            }
        };

        // var navigationRouter = App.Services.GetService<IVctoonNavigationRouter>()!;

        UpdateSelectItem(_vm.Router?.CurrentPage);
        // WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) => { UpdateSelectItem(m.Path); });
        _vm.Router.Navigated += (_, e) => { UpdateSelectItem(e?.To); };
    }

    public void UpdateSelectItem(object navigationRouterPageModel)
    {
        var model = navigationRouterPageModel as NavigationRouterPageModel;
        if (model?.Path is { } path)
            Menu.SelectedItem = _vm.NavigationMenuItemProvider.GetMenuItemByPath(path);
        else Menu.SelectedItem = null;
    }


    public void SetRootBorderHasPadding(bool hasPadding)
    {
        if (hasPadding)
        {
            this.Border.Padding = new Thickness(8, 0, 8, 0);
        }
        else
        {
            this.Border.Padding = new Thickness();
        }
    }

    private async void NavigationToCreateLibraryView_ClickAsync(object? sender, RoutedEventArgs e)
    {
        await App.Router.NavigateToAsync(App.Services.GetRequiredService<LibraryCreateView>());
    }
}