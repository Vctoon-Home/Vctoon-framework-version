using System.IO;

namespace VctoonCore.Resources;

public interface IResourceAppService : IRemoteService
{
    Task<Stream> GetComicImageStream(Guid comicImageId, int? width = null, int? height = null);
}