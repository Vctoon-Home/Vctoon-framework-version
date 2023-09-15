using System;
using System.Collections.Generic;
using VctoonClient.Dialogs;
using VctoonClient.Extensions;
using VctoonClient.Navigations.Query;
using VctoonCore.Localization;
using VctoonCore.Resources;
using VctoonCore.Resources.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Validation;

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

    public string Title => TagId == null ? L.GetResource<LibraryResource>()["AddTag"] : L.GetResource<LibraryResource>()["EditTag"];


    public TagCreateUpdateViewModel()
    {
        _tagService = App.Services.GetRequiredService<ITagAppService>();
    }

    public async Task Submit()
    {
        using var _ = Dialog.ShowContentLoading();

        try
        {
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
        catch (AbpRemoteCallException e)
        {
            e.Notify();
        }
        catch (AbpValidationException e)
        {
            e.Notify();
        }


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