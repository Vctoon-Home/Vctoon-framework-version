using System.IO;
using System.Threading.Tasks;

namespace VctoonCore.Resources;

public interface IResourceAppService
{
    Task<Stream> GetComicImage(Guid comicImageId, int? width = null, int? height = null);
}