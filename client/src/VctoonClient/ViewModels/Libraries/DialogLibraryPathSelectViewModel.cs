using System;
using System.Collections.ObjectModel;
using VctoonClient.Dialogs;
using VctoonCore.Systems;

namespace VctoonClient.ViewModels.Libraries;

public partial class DialogLibraryPathSelectViewModel : ViewModelBase, ITransientDependency
{
    private readonly ISystemAppService _systemAppService;

    [ObservableProperty]
    private ObservableCollection<SystemFolderDto> _folders = new();

    public DialogLibraryPathSelectViewModel(ISystemAppService systemAppService)
    {
        _systemAppService = systemAppService;
        Initialize();
    }

    public async void Initialize()
    {
        await LoadData();
    }


    public async Task LoadData(SystemFolderDto folder = null)
    {
        var dialogManager = (DialogManager ?? App.Services.GetRequiredService<DialogManager>());

        using var loading = dialogManager.ShowLoading();

        try
        {
            var folders = await _systemAppService.GetSystemFolder(folder?.Path);

            _folders = new ObservableCollection<SystemFolderDto>(folders);
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
    }
}