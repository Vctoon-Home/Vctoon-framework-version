using VctoonCore.Enums;

namespace VctoonCore.Libraries.Dtos;

[Serializable]
public class CreateUpdateLibraryDto
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string[] Paths { get; set; }

    public LibraryType LibraryType { get; set; }
}
