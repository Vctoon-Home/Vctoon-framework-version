using Avalonia.Controls;
using Avalonia.Input;
using VctoonClient.ViewModels.Tags;

namespace VctoonClient.Views.Tags;

public partial class TagView : UserControl, ITransientDependency
{
    private readonly TagViewModel _vm;
    public TagView()
    {
        InitializeComponent();

        _vm = App.Services.GetRequiredService<TagViewModel>();
        DataContext = _vm;
    }
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {

        if (e.GetCurrentPoint(this).Properties.PointerUpdateKind != PointerUpdateKind.LeftButtonPressed)
            return;


        var border = sender as Label;

        if (border is null)
            return;

        var tagDtoViewModel = border.DataContext as TagDtoViewModel;

        if (tagDtoViewModel is null)
            return;
        tagDtoViewModel.Selected = !tagDtoViewModel.Selected;
    }
}