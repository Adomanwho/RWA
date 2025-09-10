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

        [HttpGet("[action]")]
        public ActionResult<int> GetLogCount()
        {
            try
            {
                var count = _logRepo.GetLogCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LogController.GetLogCount", "Error getting log count", ex.Message);
                return StatusCode(500, "An error occurred while retrieving log count.");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Log>> GetLatest([FromQuery] int count = 10)
        {
            try
            {
                if (count <= 0)
                {
                    return BadRequest("Count must be greater than 0.");
                }

                var latestLogs = _logRepo.GetLatest(count);
                return Ok(latestLogs);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LogController.GetLatest", $"Error retrieving latest {count} logs", ex.Message);
                return StatusCode(500, "An error occurred while retrieving latest logs.");
            }
        }
    }
}
