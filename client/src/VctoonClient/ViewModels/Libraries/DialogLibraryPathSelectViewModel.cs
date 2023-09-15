using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VctoonCore.Systems;

namespace VctoonClient.ViewModels.Libraries;

public partial class DialogLibraryPathSelectViewModel : ViewModelBase, ITransientDependency
{
    private readonly ISystemAppService _systemAppService;

    [ObservableProperty]
    private ObservableCollection<SystemFolderDtoViewModel> folders = new ObservableCollection<SystemFolderDtoViewModel>();

    [ObservableProperty]
    private SystemFolderDtoViewModel? selectedFolder;

    [ObservableProperty]
    private bool isLoading;

    public string CurrentIdentifier { get; set; } = nameof(DialogLibraryPathSelectViewModel);

    public DialogLibraryPathSelectViewModel(ISystemAppService systemAppService)
    {
        _systemAppService = systemAppService;
        Initialize();
    }

    public async void Initialize()
    {
        Folders = await LoadData();
    }


    public async Task<ObservableCollection<SystemFolderDtoViewModel>> LoadData(string path = null)
    {
        ObservableCollection<SystemFolderDtoViewModel>? folderDtos = null;
        IsLoading = true;
        try
        {
            folderDtos =
                ObjectMapper.Map<List<SystemFolderDto>, ObservableCollection<SystemFolderDtoViewModel>>(
                    // TODO: remove ?? @$"D:\\"
                    await _systemAppService.GetSystemFolder(path ?? @$"E:\\"));

            foreach (var systemFolderDto in folderDtos)
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

                    systemFolderDto.PropertyChanged += async (sender, args) =>
                    {
                        if (args.PropertyName == nameof(SystemFolderDtoViewModel.IsExpanded) && systemFolderDto.IsExpanded)
                        {
                            systemFolderDto.Children = await LoadData(systemFolderDto.Path);
                        }
                    };

                }
            }
        }
        catch (Exception e)
        {

            App.NotificationManager.Show(new Notification("", e.Message, NotificationType.Error));
        }
        IsLoading = false;
        return folderDtos ?? new ObservableCollection<SystemFolderDtoViewModel>();
    }

    public async Task Submit()
    {
    }
}