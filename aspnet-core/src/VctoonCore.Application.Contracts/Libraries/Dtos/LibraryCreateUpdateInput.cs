using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VctoonCore.Enums;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;

namespace VctoonCore.Libraries.Dtos;

[Serializable]
public class LibraryCreateUpdateInput
{
    [Required]
    public string Name { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MinLength(1)]
    public string[] Paths { get; set; }

    public LibraryType LibraryType { get; set; }
}