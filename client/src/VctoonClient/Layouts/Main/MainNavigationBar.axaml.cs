using System.Linq;
using Avalonia;
using Avalonia.Controls;
using VctoonClient.ViewModels;

namespace VctoonClient.Layouts.Main;

public partial class MainNavigationBar : UserControl
{
    public MainNavigationBar()
    {
        InitializeComponent();

        var vm = App.Services.GetService<MainViewModel>();
        this.DataContext = vm;

        Menu.SelectedItem = Menu.Items.First();

        SetBorderPadding(!Menu.IsClosed);
        Menu.PropertyChanged += (sender, args) =>
        {
            if (args.Property.Name == nameof(Menu.IsClosed))
            {
                SetBorderPadding(args.NewValue is false);
            }
        };
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