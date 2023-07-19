using VctoonCore.Libraries;

namespace VctoonCore.Handlers;

public interface IScanHandler : ITransientDependency
{
    Task Handler(LibraryPath libraryPath);
}