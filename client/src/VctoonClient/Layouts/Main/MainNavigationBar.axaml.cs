using System.Linq;
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

    }
}