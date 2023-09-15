using System.Collections.Generic;
using Avalonia;
using VctoonClient.Views.Tags;
using VctoonCore.Resources.Dtos;

namespace VctoonClient.ViewModels.Tags;

public partial class TagDtoViewModel : ObservableObject
{

    public TagDto Tag { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderThickness))]
    private bool selected;

    public Thickness BorderThickness => Selected ? new Thickness(2) : new Thickness();



    public async void ToEditView(TagDto tag)
    {
        await App.Router.NavigateToAsync(App.Services.GetRequiredService<TagCreateUpdateView>(), new Dictionary<string, object>()
        {
            {nameof(TagCreateUpdateViewModel.TagId), tag.Id},
            {nameof(TagCreateUpdateViewModel.Tag), Tag}
        });
    }



}