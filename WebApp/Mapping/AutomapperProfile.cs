using AutoMapper;
using WebApp.ViewModels;

namespace WebApp.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, VMUser>();
        }
    }
}
