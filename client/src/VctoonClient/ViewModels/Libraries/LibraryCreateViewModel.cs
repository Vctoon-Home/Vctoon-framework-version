using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Controls.Notifications;
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
        // if (CanCreate() == false)
        // {
        //     return;
        // }

        try
        {
            await _libraryAppService.CreateAsync(Library);
        }
        catch (AbpValidationException ve)
        {
            var msg = ve.ValidationErrors.FirstOrDefault()?.ErrorMessage;
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("异常", e.Message, NotificationType.Error));
        }
    }

    public bool CanCreate()
    {
        return Validator.TryValidateObject(Library, new ValidationContext(Library), null, true);
    }


    public async void Cancel()
    {
        await App.Router.BackAsync();
    }
}