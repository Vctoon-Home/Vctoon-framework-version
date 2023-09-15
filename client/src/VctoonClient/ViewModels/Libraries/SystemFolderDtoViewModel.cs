using System.Collections.ObjectModel;

namespace VctoonClient.ViewModels.Libraries;

public partial class SystemFolderDtoViewModel : ObservableObject
{
    public string Name { get; set; }

    public string Path { get; set; }

    public bool HasChildren { get; set; }

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    private ObservableCollection<SystemFolderDtoViewModel> children;


}