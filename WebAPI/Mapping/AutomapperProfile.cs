using AutoMapper;
using BL.BLModels;
using WebAPI.DTOs;

namespace WebAPI.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, DTOs.UserDTO>();

            CreateMap<BL.BLModels.BLBook, DTOs.BookDTO>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name));// za svaki bookDTO umece u GenreName ime Genre-a

            CreateMap<DTOs.BookDTO, BL.BLModels.BLBook>();

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            CreateMap<BL.BLModels.BLGenre, DTOs.GenreDTO>();

            CreateMap<BL.BLModels.BLLocation, DTOs.LocationDTO>();
        }
    }
}
