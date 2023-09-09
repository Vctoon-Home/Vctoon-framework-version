using Avalonia.Controls;
using VctoonClient.ViewModels.Libraries;

namespace VctoonClient.Views.Libraries;

public partial class LibraryCreateUpdateView : UserControl, ITransientDependency
{
    private readonly LibraryCreateUpdateViewModel _vm;

    public LibraryCreateUpdateView()
    {
        InitializeComponent();
        _vm = App.Services.GetRequiredService<LibraryCreateUpdateViewModel>();
        this.DataContext = _vm;
    }
}