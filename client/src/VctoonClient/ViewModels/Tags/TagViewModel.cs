using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VctoonClient.Dialogs;
using VctoonClient.Extensions;
using VctoonClient.Navigations.Query;
using VctoonClient.Views.Tags;
using VctoonCore.Resources;
using Volo.Abp.Http.Client;

namespace VctoonClient.ViewModels.Tags;

public partial class TagViewModel : ViewModelBase, ITransientDependency, IQueryAttributable
{

    private readonly ITagAppService _tagAppService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelected), nameof(SelectedTags))]
    private ObservableCollection<TagDtoViewModel> tags = new ObservableCollection<TagDtoViewModel>();

    public bool HasSelected => Tags.Any(t => t.Selected);


    public ObservableCollection<TagDtoViewModel> SelectedTags => new ObservableCollection<TagDtoViewModel>(Tags.Where(x => x.Selected));


    public TagViewModel(ITagAppService tagAppService)
    {
        _tagAppService = tagAppService;
        Initialize();

        Tags.CollectionChanged += (sender, args) =>
        {
            UpdateProperties();
        };
    }

    public async void Initialize()
    {
        await LoadData();
    }


    public async Task LoadData()
    {
        using var _ = Dialog.ShowContentLoading();

        try
        {
            var tagDtos = await _tagAppService.GetAllAsync();
            Tags = new ObservableCollection<TagDtoViewModel>(tagDtos.Select(t => new TagDtoViewModel()
            {
                Tag = t
            }));

            foreach (var tagDtoViewModel in Tags)
            {
                tagDtoViewModel.PropertyChanged += (sender, args) =>
                {
                    UpdateProperties();
                };
            }
        }
        catch (AbpRemoteCallException e)
        {
            e.Notify();
        }


    }


    public async void ToCreateView()
    {
        await App.Router.NavigateToAsync(App.Services.GetRequiredService<TagCreateUpdateView>());
    }

    public async Task DeleteSelected()
    {

        var confirm = await Dialog.ShowConfirmAsync(options: opt =>
        {
            // TODO: to localized
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
    public async void ApplyQueryAttributes(Dictionary<string, object>? paras, bool isBack)
    {

        if (paras is null)
            return;

        if (isBack && paras.TryGetValue("Succeeded", out var success) && (bool) success)
        {
            await LoadData();
        }
    }

    public void UpdateProperties()
    {
        OnPropertyChanged(nameof(HasSelected));
        OnPropertyChanged(nameof(SelectedTags));
    }


}