using Avalonia.Controls;
using VctoonClient.ViewModels.Tags;

namespace VctoonClient.Views.Tags;

public partial class TagCreateUpdateView : UserControl, ITransientDependency
{
    private readonly TagCreateUpdateViewModel _vm;
    public TagCreateUpdateView()
    {
        InitializeComponent();

        _vm = App.Services.GetRequiredService<TagCreateUpdateViewModel>();
        DataContext = _vm;
    }
}