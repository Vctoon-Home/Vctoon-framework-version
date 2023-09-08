using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using EasyDialog.Avalonia.Dialogs;
using Ursa.Controls;
using VctoonClient.ViewModels.Libraries;

namespace VctoonClient.Views.Libraries;

public partial class DialogLibraryPathSelectView : UserControl, ITransientDependency
{
    private readonly DialogLibraryPathSelectViewModel _vm;

    public DialogLibraryPathSelectView()
    {
        InitializeComponent();
        
        
        _vm = App.Services.GetRequiredService<DialogLibraryPathSelectViewModel>();
        this.DataContext = _vm;
        this.UseEasyLoading(_vm.CurrentIdentifier);
    }
}