using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class HomeView : UserControl, ITransientDependency
{
    private readonly HomeViewModel _vm;

    public HomeView(HomeViewModel vm)
    {
        _vm = vm;
        InitializeComponent();

        DataContext = vm;
    }
}