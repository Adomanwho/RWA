using BL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenRepository _tokenRepo;

        public TokenController(IConfiguration config, ITokenRepository tokenRepo)
        {
            _config = config;
            _tokenRepo = tokenRepo;
        }

        [HttpGet("[action]")]   
        public ActionResult GetToken()
        {
            string secureKey = _config["JWT:SecureKey"];
            string token = _tokenRepo.GetToken(secureKey);
            return Ok(token);
        }

    }
}
