using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VctoonCore.Resources;

namespace VctoonCore.Controllers;

public class ResourceController : VctoonCoreController
{
    private readonly IResourceAppService _resourceAppService;

    public ResourceController(IResourceAppService resourceAppService)
    {
        _resourceAppService = resourceAppService;
    }

    [HttpGet("comic-image")]
    public async Task<IActionResult> GetComicImage(Guid comicImageId, int? width = null, int? height = null)
    {
        var stream = await _resourceAppService.GetComicImageStream(comicImageId, width, height);

        return new FileStreamResult(stream, "application/octet-stream")
        {
            FileDownloadName = "test.jpg"
        };
    }
}