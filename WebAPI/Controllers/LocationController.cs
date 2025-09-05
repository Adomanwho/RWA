using AutoMapper;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILocationRepository _locationRepo;
        private readonly RwaLibraryContext _dbContext;

        public LocationController(IMapper mapper, ILocationRepository locationRepo, RwaLibraryContext dbContext)
        {
            _mapper = mapper;
            _locationRepo = locationRepo;
            _dbContext = dbContext;
        }

        [HttpGet("[action]")] // /api/Location/GetLocations
        public ActionResult<IEnumerable<LocationDTO>> GetLocations()
        {
            try
            {
                var locations = _locationRepo.GetLocations();
                var dtoLocations = _mapper.Map<IEnumerable<LocationDTO>>(locations);
                return Ok(dtoLocations);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.GetLocations", $"Error retrieving locations: {ex.Message}");
                throw;
            }
        }

        [HttpGet("{name}/[action]")] // /api/Location/{name}/GetByName
        public ActionResult<LocationDTO> GetByName(string name)
        {
            try
            {
                var location = _locationRepo.GetByName(name);
                if (location == null) return NotFound($"Location \"{name}\" not found.");

                var dtoLocation = _mapper.Map<LocationDTO>(location);
                return Ok(dtoLocation);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.GetByName", $"Error retrieving location \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpPost("[action]")] // /api/Location/Create
        public ActionResult<LocationDTO> Create([FromBody] string name)
        {
            try
            {
                var location = _locationRepo.Create(name);
                if (location == null) return Conflict($"Location \"{name}\" already exists.");

                var dtoLocation = _mapper.Map<LocationDTO>(location);
                return CreatedAtAction(nameof(Create), new { id = location.Id }, dtoLocation);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Create", $"Error creating location \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpPut("{name}/[action]")] // /api/Location/{name}/Update
        public IActionResult Update(string name, [FromBody] string newName)
        {
            try
            {
                string result = _locationRepo.Update(name, newName);

                if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);

                if (result.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                    return Conflict(result);

                return NoContent();
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Update", $"Error updating location \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{name}/[action]")] // /api/Location/{name}/Delete
        public IActionResult Delete(string name)
        {
            try
            {
                bool success = _locationRepo.Delete(name);
                if (!success) return NotFound($"Location \"{name}\" not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Delete", $"Error deleting location \"{name}\": {ex.Message}");
                throw;
            }
        }
    }
}
