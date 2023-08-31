using Avalonia.Controls;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private readonly MainViewModel _vm;


    public MainView()
    {
        InitializeComponent();

        _vm = App.Services.GetService<MainViewModel>()!;
        this.DataContext = _vm;
    }
}