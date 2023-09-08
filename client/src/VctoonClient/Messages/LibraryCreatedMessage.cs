using VctoonCore.Libraries.Dtos;

namespace VctoonClient.Messages;

public class LibraryCreatedMessage
{
    public LibraryDto Library { get; set; }
}