using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VctoonClient.ViewModels.Tags;

namespace VctoonClient.Views.Tags;

public partial class TagCreateUpdateView : UserControl, ITransientDependency
{
    private readonly TagCreateUpdateViewModel _vm;
    public TagCreateUpdateView()
    {
        InitializeComponent();

        _vm = App.Services.GetRequiredService<TagCreateUpdateViewModel>();
        this.DataContext = _vm;
    }
}