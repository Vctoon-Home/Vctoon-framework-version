using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Volo.Abp;

namespace VctoonCore.Resources;

[RemoteService(IsEnabled = false)]
public class ResourceAppService : VctoonCoreAppService, IResourceAppService
{
    private readonly IComicImageRepository _comicImageRepository;

    public ResourceAppService(IComicImageRepository comicImageRepository)
    {
        _comicImageRepository = comicImageRepository;
    }

    public async Task<Stream> GetComicImage(Guid comicImageId, int? width = null, int? height = null)
    {
        await CheckPermissionAsync();

        if (comicImageId == Guid.Empty) throw new BusinessException("comicImageId is empty");

        var comicImage = await _comicImageRepository.FindAsync(comicImageId);

        if (comicImage == null)
        {
            throw new BusinessException("image not found");
        }

        var stream = comicImage.GetImageStream();

        if (stream == null)
        {
            throw new BusinessException("image not found");
        }

        if (!width.HasValue || !height.HasValue) return stream;

        var image = await Image.LoadAsync(stream);
        image.Mutate(x => x.Resize(width.Value, height.Value));

        return stream;
    }


    // check permission

    public async Task CheckPermissionAsync()
    {
        // throw new BusinessException("permission denied");
    }
}