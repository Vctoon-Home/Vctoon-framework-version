using System;
using DialogHostAvalonia;
using VctoonClient.Helpers;
using VctoonClient.Views.Libraries;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateInputViewModel _library = new();

    public LibraryCreateViewModel(ILibraryAppService libraryAppService)
    {
        _libraryAppService = libraryAppService;

        Library.ErrorsChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };

        Library.PropertyChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };
        Library.Paths.CollectionChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };
    }

    public async void CreateLibrary()
    {
        if (!ValidHelper.IsValid(Library))
            return;
        try
        {
            await _libraryAppService.CreateAsync(
                ObjectMapper.Map<LibraryCreateUpdateInputViewModel, LibraryCreateUpdateInput>(Library));
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }

    public bool CanCreateLibrary()
    {
        return ValidHelper.IsValid(Library);
    }

    public async void ShowAddFolderDialog()
    {
        var dialogViewModel = App.Services.GetRequiredService<DialogLibraryPathSelectView>();
        var res = await DialogManager.ShowCloseOnClickAwayAsync<DialogLibraryPathSelectViewModel>(dialogViewModel);
    }

    public void RemoveFolder(string path)
    {
        Library.Paths.Remove(path);
    }


    public async void Cancel()
    {
        await App.Router.BackAsync();
    }
}