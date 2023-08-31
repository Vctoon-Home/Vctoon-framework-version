using System;
using System.ComponentModel.DataAnnotations;
using Abp.Localization.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using VctoonClient.Views;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;
using Volo.Abp.Validation;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateDto _library = new ();

    public LibraryCreateViewModel(ILibraryAppService libraryAppService)
    {
        _libraryAppService = libraryAppService;
    }

    public async void Create()
    {
        try
        {
            await _libraryAppService.CreateAsync(Library);
        }
        catch (AbpValidationException ve)
        {
            var notification = new Notification("验证错误", ve.ValidationErrors.ToString(), NotificationType.Error);
            App.NotificationManager.Show(notification);
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("异常", e.Message, NotificationType.Error));
        }
    }

    public async void Cancel()
    {
        await App.Router.BackAsync();
    }
}