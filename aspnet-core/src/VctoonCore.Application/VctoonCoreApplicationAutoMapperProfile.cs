using System.Linq;
using VctoonCore.Libraries;
using VctoonCore.Libraries.Dtos;
using AutoMapper;
using VctoonCore.Resources;
using Volo.Abp.AutoMapper;

namespace VctoonCore;

public class VctoonCoreApplicationAutoMapperProfile : Profile
{
    public VctoonCoreApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Library, LibraryDto>()
            .ForMember(b => b.Paths, opt => opt.MapFrom(b => b.Paths.Select(b => b.Path).ToArray()));

        CreateMap<LibraryCreateUpdateInput, Library>(MemberList.Source)
            .Ignore(b => b.Paths);


        CreateMap<Tag, TagDto>();
        CreateMap<TagDto, Tag>();

        CreateMap<Comic, ComicDto>();
        CreateMap<ComicUpdateDto, Comic>();

        CreateMap<ComicChapter, ComicChapterDto>();
        CreateMap<ComicChapterUpdateDto, ComicChapter>();

        CreateMap<ComicImage, ComicImageDto>();
        CreateMap<ComicImageUpdateDto, ComicImage>();

        CreateMap<Video, VideoDto>();
        CreateMap<VideoDto, Video>();


    }
}
