using System;
using DialogHostAvalonia;
using VctoonClient.Helpers;
using VctoonClient.Messages;
using VctoonClient.Views.Libraries;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateInputViewModel library = new();

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

    public bool CanCreateLibrary()
    {
        return ValidHelper.IsValid(Library);
    }

    public async void AddFolderWithDialog()
    {
        var pathSelectView = App.Services.GetRequiredService<DialogLibraryPathSelectView>();
 
        var path = await DialogService.ShowAsync<string>(pathSelectView, options:
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