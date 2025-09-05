using AutoMapper;
using BL.BLModels;
using WebApp.ViewModels;

namespace WebApp.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, VMUser>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<BLBook, VMBook>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<VMBook, BLBook>()
            .ForMember(dest => dest.Genre, opt => opt.Ignore());
        }
    }
}
