namespace VctoonCore.Libraries.Dtos;

[Serializable]
public class LibraryGetListInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

}