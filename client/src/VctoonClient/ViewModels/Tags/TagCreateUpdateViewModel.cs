using System;
using System.Collections.Generic;
using VctoonClient.Dialogs;
using VctoonClient.Navigations.Query;
using VctoonCore.Resources;
using VctoonCore.Resources.Dtos;

namespace VctoonClient.ViewModels.Tags;

[QueryProperty(nameof(TagId))]
[QueryProperty(nameof(Tag))]
public partial class TagCreateUpdateViewModel : ViewModelBase, ITransientDependency
{
    [ObservableProperty]
    private TagDto tag = new TagDto();

    [ObservableProperty]
    private Guid? tagId;

    private readonly ITagAppService _tagService;


    public TagCreateUpdateViewModel()
    {
        _tagService = App.Services.GetRequiredService<ITagAppService>();
    }

    public async Task Submit()
    {
        using var _ = Dialog.ShowContentLoading();

        if (TagId.HasValue)
        {
            await Update();
        }
        else
        {
            await Create();
        }

        await App.Router.BackAsync(new Dictionary<string, object>()
        {
            {"Succeeded", true}
        });
    }

    public async Task Create()
    {
        await _tagService.CreateAsync(Tag);
    }

    public async Task Update()
    {
        await _tagService.UpdateAsync(TagId!.Value, Tag);
    }


}