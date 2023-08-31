using System.ComponentModel.DataAnnotations;
using VctoonCore.Enums;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateUpdateInputViewModel : ObservableObject
{
    [Required]
    [MinLength(50)]
    [ObservableProperty]
    private string _name;

    [Required]
    [MinLength(1)]
    [ObservableProperty]
    private string[] _paths;

    [ObservableProperty]
    private LibraryType _libraryType;
}