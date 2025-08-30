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

            CreateMap<BL.BLModels.BLBook, DTOs.BookDTO>();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            CreateMap<DTOs.BookDTO, BL.BLModels.BLBook>();
        }
    }
}
