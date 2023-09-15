using VctoonCore.Enums;
using VctoonCore.Libraries;

namespace VctoonCore.Resources.Handlers;

public interface IScanHandler : ITransientDependency
{
    public LibraryType SupportLibraryType { get; set; }
    Task Handler(LibraryPath libraryPath);
}