﻿using System.Linq;
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

        CreateMap<CreateUpdateLibraryDto, Library>(MemberList.Source)
            .Ignore(b => b.Paths);


        CreateMap<Tag, TagDto>();
        CreateMap<Tag, TagDto>();
    }
}