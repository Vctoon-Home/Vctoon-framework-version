using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class HomeView : UserControl, ITransientDependency
{
    private readonly HomeViewModel _vm;

    public HomeView()
    {
        _vm = App.Services.GetService<HomeViewModel>();
        InitializeComponent();

        DataContext = _vm;
    }
}