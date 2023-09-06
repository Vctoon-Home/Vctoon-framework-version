using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels.Libraries;

namespace VctoonClient.Views.Libraries;

public partial class DialogLibraryPathSelectView : UserControl, ITransientDependency
{
    public DialogLibraryPathSelectView()
    {
        InitializeComponent();

        this.DataContext = App.Services.GetRequiredService<DialogLibraryPathSelectViewModel>();
    }
}