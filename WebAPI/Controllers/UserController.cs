using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserController(IUserRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            var blUsers = _userRepo.GetAll();
            var apiUsers = _mapper.Map<IEnumerable<UserDTO>>(blUsers);
            return Ok(apiUsers);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult Register([FromForm] UserDTO user)
        {
            bool registered = _userRepo.RegisterUser(user.Name, user.Email, user.Password);// role id je automatski 2 jer je admin aplikacija

            if (!registered) return Conflict("This user already exists.");

            return Created();
        }

        [HttpPost("[action]")]
        [AllowAnonymous]

        public ActionResult<string> Login([FromForm] UserDTO user)
        {
            string logged = _userRepo.LoginUser(user.Name, user.Password, _config["JWT:SecureKey"]);

            if (string.IsNullOrEmpty(logged)) return Unauthorized("The provided user doesn't exist.");
            else if (logged[0] == 'T') return Unauthorized(logged);
            return Ok(logged);
        }
    }
}
