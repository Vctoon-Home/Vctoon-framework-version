using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyDialog.Avalonia.Dialogs;
using VctoonClient.Dialogs;
using VctoonCore.Systems;

namespace VctoonClient.ViewModels.Libraries;

public partial class DialogLibraryPathSelectViewModel : ViewModelBase, ITransientDependency
{
    private readonly ISystemAppService _systemAppService;

    [ObservableProperty]
    private ObservableCollection<SystemFolderDtoViewModel> folders = new();

    [ObservableProperty]
    private SystemFolderDtoViewModel selectedFolder;


    [ObservableProperty]
    private bool isLoading;

    public string CurrentIdentifier { get; set; } = nameof(DialogLibraryPathSelectViewModel);

    public DialogLibraryPathSelectViewModel(ISystemAppService systemAppService)
    {
        this._systemAppService = systemAppService;
        Initialize();
    }

    public async void Initialize()
    {
        await LoadData();
    }


    public async Task LoadData(SystemFolderDto folder = null)
    {
        var dialogService = DialogService;
        IsLoading = true;
        try
        {
            var folders =
                ObjectMapper.Map<List<SystemFolderDto>, ObservableCollection<SystemFolderDtoViewModel>>(
                    // TODO: remove ?? @$"D:\\"
                    await _systemAppService.GetSystemFolder(folder?.Path ?? @$"D:\\"));

            foreach (var systemFolderDto in folders)
            {
                if (systemFolderDto.HasChildren)
                {
                    systemFolderDto.Children = new ObservableCollection<SystemFolderDtoViewModel>()
                    {
                        new SystemFolderDtoViewModel()
                        {
                            Name = "Loading...",
                            HasChildren = false
                        }
                    };
                }
            }

            Folders = folders;
        }
        catch (Exception e)
        {
            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }

        IsLoading = false;
    }

    public async Task Submit()
    {
    }
}