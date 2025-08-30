using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepo;
        private readonly RwaLibraryContext _dbContext;

        public LogController(ILogRepository logRepo, RwaLibraryContext dbContext)
        {
            _logRepo = logRepo;
            _dbContext = dbContext;
        }

        [HttpGet("[action]")]
        public ActionResult<PagedResult<Log>> GetPaged([FromQuery] int? level, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var pagedResult = _logRepo.GetPaged(level, page, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LogController.GetPaged", "Error reading logs", ex.Message);
                throw;
            }
        }

    }
}
