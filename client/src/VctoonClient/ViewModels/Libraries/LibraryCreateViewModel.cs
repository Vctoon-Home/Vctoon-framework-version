using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Controls.Notifications;
using VctoonClient.Helpers;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;
using Volo.Abp.Validation;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateInput _library = new();


    public LibraryCreateViewModel(ILibraryAppService libraryAppService)
    {
        _libraryAppService = libraryAppService;
    }


    public async void Create()
    {
        if (!ValidHelper.IsValid(Library))
            return;
        try
        {
            await _libraryAppService.CreateAsync(Library);
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }

    public async void Cancel()
    {
        await App.Router.BackAsync();
    }
}