using AutoMapper;
using VctoonClient.ViewModels.Libraries;
using VctoonCore.Libraries.Dtos;

namespace VctoonClient;

public class VctoonClientAutoMapperProfile : Profile
{
    public VctoonClientAutoMapperProfile()
    {
        CreateMap<LibraryCreateUpdateInput, LibraryCreateUpdateInputViewModel>();
        CreateMap<LibraryCreateUpdateInputViewModel, LibraryCreateUpdateInput>();
    }
}