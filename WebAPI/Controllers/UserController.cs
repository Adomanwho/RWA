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
        public UserController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public IEnumerable<UserDTO> GetAll()
        {
            var blUsers = _userRepo.GetAll();
            var apiUsers = _mapper.Map<IEnumerable<UserDTO>>(blUsers);
            return apiUsers;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}/[action]")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("[action]")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}/[action]")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}/[action]")]
        public void Delete(int id)
        {
        }
    }
}
