using System.Collections.Generic;
using System.Collections.ObjectModel;
using VctoonCore.Systems;

namespace VctoonClient.ViewModels.Libraries;

public partial class SystemFolderDtoViewModel : ObservableObject
{
    public string Name { get; set; }

    public string Path { get; set; }

    public bool HasChildren { get; set; }

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    ObservableCollection<SystemFolderDtoViewModel> children;
}