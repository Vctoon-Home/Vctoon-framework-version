using System.ComponentModel.DataAnnotations;
using VctoonCore.Enums;

namespace VctoonCore.Libraries.Dtos;

[Serializable]
public class LibraryCreateUpdateInput
{
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<string> Paths { get; set; } = new List<string>();

    public LibraryType LibraryType { get; set; }
}