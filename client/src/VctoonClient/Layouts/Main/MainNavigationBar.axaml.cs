using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Labs.Controls;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using VctoonClient.Messages;
using VctoonClient.Navigations;
using VctoonClient.ViewModels;

namespace VctoonClient.Layouts.Main;

public partial class MainNavigationBar : UserControl
{
    public MainNavigationBar()
    {
        InitializeComponent();

        var vm = App.Services.GetService<MainViewModel>();
        this.DataContext = vm;

        SetBorderPadding(!Menu.IsClosed);
        Menu.PropertyChanged += (sender, args) =>
        {
            if (args.Property.Name == nameof(Menu.IsClosed))
            {
                SetBorderPadding(args.NewValue is false);
            }
        };

        var navigationRouter = App.Services.GetService<IVctoonNavigationRouter>()!;

        SetSelect(navigationRouter.CurrentPage);

        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) => { SetSelect(m.Path); });
    }

    public void SetSelect(object pathOrView)
    {
        if (pathOrView == null)
            return;

        if (pathOrView is string path)
            Menu.SelectedItem =
                Menu.Items.FirstOrDefault(i => i is MenuItemViewModel vm && vm.Path == path);
        else if (pathOrView is UserControl view)
        {
            Menu.SelectedItem =
                Menu.Items.FirstOrDefault(i => i is MenuItemViewModel vm && vm.ViewType == view.GetType());
        }
    }


    public void SetBorderPadding(bool set)
    {
        if (set)
        {
            this.Border.Padding = new Thickness(8, 0, 8, 0);
        }
        else
        {
            this.Border.Padding = new Thickness();
        }
    }
}