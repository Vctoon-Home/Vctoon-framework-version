using System.ComponentModel.DataAnnotations;
using VctoonCore.Enums;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateUpdateInputViewModel : ObservableValidator
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [ObservableProperty]
    string _name;

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MinLength(1)]
    [ObservableProperty]
    string[] _paths;

    [ObservableProperty]
    LibraryType _libraryType;
}