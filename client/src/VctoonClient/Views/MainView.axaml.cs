using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using VctoonClient.Stores.Users;
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


    protected override async void OnInitialized()
    {
        base.OnInitialized();

        await _vm.InitializeWhenViewIsLoaded();
    }
}