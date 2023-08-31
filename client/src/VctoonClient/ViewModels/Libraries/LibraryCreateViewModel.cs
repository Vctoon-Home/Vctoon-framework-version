using System;
using Avalonia.Controls.Notifications;
using Mapster;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;
using Volo.Abp.Validation;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateViewModel : ViewModelBase, ITransientDependency
{
    private readonly ILibraryAppService _libraryAppService;

    [ObservableProperty]
    private LibraryCreateUpdateInputViewModel _library = new();

    public LibraryCreateViewModel(ILibraryAppService libraryAppService)
    {
        _libraryAppService = libraryAppService;
    }

    public async void Create()
    {
        try
        {
            await _libraryAppService.CreateAsync(Library.Adapt<LibraryCreateUpdateInput>());
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