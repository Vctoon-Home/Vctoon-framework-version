using Avalonia.Controls;
using VctoonClient.ViewModels.Libraries;

namespace VctoonClient.Views.Libraries;

public partial class LibraryCreateView : UserControl, ITransientDependency
{
    private readonly LibraryCreateViewModel _vm;

    public LibraryCreateView()
    {
        InitializeComponent();
        _vm = App.Services.GetRequiredService<LibraryCreateViewModel>();
        this.DataContext = _vm;
    }
}