using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{

    public interface IUserRepository
    {
        public IEnumerable<BLUser> GetAll();
    }

    public class UserRepository : IUserRepository
    {
        private readonly RwaLibraryContext _dbContext = new();
        private readonly IMapper _mapper;

        public UserRepository(RwaLibraryContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<BLUser> GetAll()
        {
            var dbUsers = _dbContext.Users;
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;
        }
    }
}
