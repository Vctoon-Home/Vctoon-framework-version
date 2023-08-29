using System.Linq;
using Avalonia;
using Avalonia.Controls;
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

        WeakReferenceMessenger.Default.Register<NavigationMessage>(this, (r, m) =>
        {
            if (m.Path != null)
            {
                Menu.SelectedItem =
                    Menu.Items.FirstOrDefault(i => i is MenuItemViewModel vm && vm.Path == m.Path);
            }
        });
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