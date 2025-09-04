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
        public BLUser CheckIfUserExists(string name); // ako ne postoji vraca null, inace usera
        public bool RegisterUser(string name, string email, string password, int roleId = 2); //false ako nije uspijelo, inace true
        public string LoginUser(string name, string password, string secureKey);
    }

    public class UserRepository : IUserRepository
    {
        private readonly RwaLibraryContext _dbContext = new();
        private readonly IMapper _mapper;
        private readonly ITokenRepository _tokenRepo;

        public UserRepository(RwaLibraryContext context, IMapper mapper, ITokenRepository tokenRepo)
        {
            _dbContext = context;
            _mapper = mapper;
            _tokenRepo = tokenRepo;
        }

        public IEnumerable<BLUser> GetAll()
        {
            var dbUsers = _dbContext.Users;
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            _dbContext.WriteLog(2, "UserController.GetAll", "User retrieved.");

            return blUsers;
        }

        public BLUser CheckIfUserExists(string name)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Name == name);
            if (user == null) return null;
            var blUser = _mapper.Map<BLUser>(user);
            return blUser;
        }

        public bool RegisterUser(string name, string email, string password, int roleId = 2)
        {
            try
            {
                var blUser = CheckIfUserExists(name);
                if (blUser != null)
                {
                    _dbContext.WriteLog(2, "UserController.Register", "User tried to register with existing username.");
                    return false;
                }
                BLUser userToRegister = new BLUser
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    RoleId = roleId
                };
                User user = _mapper.Map<User>(userToRegister);
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "UserController.Register", "User registered.");

                return true;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "UserController.Register", $"Error while registering: {ex.Message}.");
                throw;
            }
        }

        public string LoginUser(string name, string password, string secureKey)
        {
            BLUser user = CheckIfUserExists(name);
            if (user == null)
            {
                _dbContext.WriteLog(2, "UserController.Login", "User tried logging into an account that doesn't exist.");
                return "";//korisnik ne postoji
            }

            if (user.Password != password)
            {
                _dbContext.WriteLog(2, "UserController.Login", "User tried logging into an account with the wrong password.");
                return "The user exists but the password is incorrect.";
            }

            _dbContext.WriteLog(2, "UserController.Login", "User logged in.");

            return $"Successful login! \nYour token is: {_tokenRepo.GetToken(secureKey)}";
        }
    }
}
