using AutoMapper;
using VctoonClient.ViewModels.Libraries;
using VctoonCore.Libraries.Dtos;
using VctoonCore.Systems;

namespace VctoonClient;

public class VctoonClientAutoMapperProfile : Profile
{
    public VctoonClientAutoMapperProfile()
    {
        CreateMap<LibraryCreateUpdateInput, LibraryCreateUpdateInputViewModel>();
        CreateMap<LibraryCreateUpdateInputViewModel, LibraryCreateUpdateInput>();

        CreateMap<SystemFolderDto, SystemFolderDtoViewModel>();

    }
}