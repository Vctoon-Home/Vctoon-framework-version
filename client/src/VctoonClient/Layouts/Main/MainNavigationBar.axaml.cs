using Avalonia.Controls;

namespace VctoonClient.Layouts.Main;

public partial class MainNavigationBar : UserControl
{
    public MainNavigationBar()
    {
        InitializeComponent();

        var vm = App.Services.GetService<MainNavigationBarViewModel>();
        this.DataContext = vm;
    }
}