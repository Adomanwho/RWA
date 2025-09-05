using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DALModels.User, BLModels.BLUser>();
            CreateMap<BL.BLModels.BLUser, DALModels.User>();

            CreateMap<DALModels.Book, BLModels.BLBook>();

            CreateMap<DALModels.Genre, BLModels.BLGenre>();

            CreateMap<DALModels.Location, BLModels.BLLocation>();
        }
    }
}
