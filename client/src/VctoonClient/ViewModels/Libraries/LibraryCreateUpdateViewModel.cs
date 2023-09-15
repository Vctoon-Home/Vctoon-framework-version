using System;
using System.Collections.Generic;
using DialogHostAvalonia;
using VctoonClient.Helpers;
using VctoonClient.Messages;
using VctoonClient.Navigations.Query;
using VctoonClient.Views.Libraries;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;
using VctoonCore.Localization;

namespace VctoonClient.ViewModels.Libraries;

[QueryProperty(nameof(Library))]
[QueryProperty(nameof(LibraryId))]
public partial class LibraryCreateUpdateViewModel : ViewModelBase, ITransientDependency
{

    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateInputViewModel library = new();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private Guid? libraryId;

    public string Title => LibraryId == null ? L.GetResource<LibraryResource>()["AddLibrary"] : L.GetResource<LibraryResource>()["EditLibrary"];

    public LibraryCreateUpdateViewModel(ILibraryAppService libraryAppService)
    {
        _libraryAppService = libraryAppService;

        Library.ErrorsChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };

        Library.PropertyChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };
        Library.Paths.CollectionChanged += (sender, args) => { OnPropertyChanged(nameof(CanCreateLibrary)); };
    }

    public void Submit()
    {
        if (LibraryId == null)
            CreateLibrary();
        else
            UpdateLibrary();
    }

    public async void CreateLibrary()
    {
        if (!ValidHelper.IsValid(Library))
            return;
        try
        {
            var library = await _libraryAppService.CreateAsync(
                ObjectMapper.Map<LibraryCreateUpdateInputViewModel, LibraryCreateUpdateInput>(Library));

            WeakReferenceMessenger.Default.Send(new LibraryCreatedMessage()
            {
                Library = library
            });

            await App.Router.BackAsync();
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }

    public async void UpdateLibrary()
    {
        if (!ValidHelper.IsValid(Library))
            return;
        try
        {
            var library = await _libraryAppService.UpdateAsync(libraryId!.Value,
                ObjectMapper.Map<LibraryCreateUpdateInputViewModel, LibraryCreateUpdateInput>(Library));

            WeakReferenceMessenger.Default.Send(new LibraryUpdatedMessage()
            {
                Library = library
            });

            await App.Router.BackAsync();
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

    public async void AddFolderWithDialog()
    {
        var pathSelectView = App.Services.GetRequiredService<DialogLibraryPathSelectView>();

        var path = await Dialog.ShowAsync<string>(pathSelectView, options:
            opt => { opt.CloseOnClickAway = true; });

        if (path.IsNullOrEmpty())
            return;

        Library.Paths.Add(path!);
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