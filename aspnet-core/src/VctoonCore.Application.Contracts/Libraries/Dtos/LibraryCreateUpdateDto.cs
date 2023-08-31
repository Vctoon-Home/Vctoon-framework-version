using System.ComponentModel.DataAnnotations;
using VctoonCore.Enums;

namespace VctoonCore.Libraries.Dtos;

[Serializable]
public class LibraryCreateUpdateDto
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MinLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MinLength(1)]
    public string[] Paths { get; set; }

    public LibraryType LibraryType { get; set; }
}