using VctoonCore.Libraries;

namespace VctoonCore.Resources.Handlers;

public interface IScanHandler : ITransientDependency
{
    Task Handler(LibraryPath libraryPath);
}