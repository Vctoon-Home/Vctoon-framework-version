using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VctoonClient.Dialogs;
using VctoonClient.Navigations.Query;
using VctoonClient.Views.Tags;
using VctoonCore.Resources;
using VctoonCore.Resources.Dtos;

namespace VctoonClient.ViewModels.Tags;

public partial class TagViewModel : ViewModelBase, ITransientDependency, IQueryAttributable
{

    private readonly ITagAppService _tagAppService;

    [ObservableProperty]
    ObservableCollection<TagDtoViewModel> tags;


    [ObservableProperty]
    private bool hasSelected;

    public TagViewModel(ITagAppService tagAppService)
    {
        _tagAppService = tagAppService;

        LoadData();
    }


    public async Task LoadData()
    {
        using var _ = Dialog.ShowContentLoading();

        var tags = await _tagAppService.GetAllAsync();
        // var tags = await MockData();

        Tags = new ObservableCollection<TagDtoViewModel>(tags.Select(t => new TagDtoViewModel()
        {
            Tag = t
        }));
        HasSelected = false;
    }

    public async Task<List<TagDto>> MockData()
    {
        var tags = new List<TagDto>();
        for (int i = 0; i < 10; i++)
        {
            tags.Add(new TagDto()
            {
                Id = Guid.NewGuid(),
                Name = $"Tag {i}"
            });
        }

        return tags;
    }


    public async void ToCreateView()
    {
        await App.Router.NavigateToAsync(App.Services.GetRequiredService<TagCreateUpdateView>());
    }

    public async Task DeleteSelected()
    {

        var confirm = await Dialog.ShowConfirmAsync(options: opt =>
        {
            opt.Message = "Are you sure to delete selected tags?";
            opt.ConfirmBtnText = "Yes";
            opt.CancelBtnText = "No";

        });

        if (!confirm) return;


        var selectedIds = Tags.Where(x => x.Selected).Select(s => s.Tag.Id).ToArray();
        if (selectedIds.IsNullOrEmpty())
        {
            return;
        }

        using var _ = Dialog.ShowContentLoading();

        await _tagAppService.Deletes(selectedIds);

        await LoadData();

    }
    public void ApplyQueryAttributes(Dictionary<string, object>? paras, bool isBack)
    {
        if (isBack && paras.TryGetValue("Succeeded", out var success) && (bool) success)
        {
            LoadData();
        }
    }

}