using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VctoonCore.Enums;

namespace VctoonClient.ViewModels.Libraries;

public partial class LibraryCreateUpdateInputViewModel : ObservableValidator
{
    [Required]
    [ObservableProperty]
    string name;

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MaxLength(1)]
    public ObservableCollection<string> Paths { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    LibraryType _libraryType;
}