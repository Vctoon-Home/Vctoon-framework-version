using Avalonia.Controls;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.ViewModels;

namespace VctoonClient.Views;

public partial class MainView : UserControl, ISingletonDependency
{
    private readonly MainViewModel _vm;


    public MainView()
    {
        InitializeComponent();
        this.UseEasyLoading().UseEasyDialog();
        _vm = App.Services.GetService<MainViewModel>()!;
        DataContext = _vm;
    }
}