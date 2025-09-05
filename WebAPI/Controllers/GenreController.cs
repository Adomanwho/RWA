using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenreController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepo;
        private readonly RwaLibraryContext _dbContext;

        public GenreController(IMapper mapper, IGenreRepository genreRepo, RwaLibraryContext dbContext)
        {
            _mapper = mapper;
            _genreRepo = genreRepo;
            _dbContext = dbContext;
        }

        [HttpGet("[action]")] // /api/Genre/GetGenres
        public ActionResult<IEnumerable<GenreDTO>> GetGenres()
        {
            try
            {
                IEnumerable<BLGenre> genres = _genreRepo.GetGenres();
                var dtoGenres = _mapper.Map<IEnumerable<GenreDTO>>(genres);
                return Ok(dtoGenres);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.GetGenres", $"Error retrieving genres: {ex.Message}");
                throw;
            }
        }

        [HttpGet("{name}/[action]")] // /api/Genre/{name}/GetByName
        public ActionResult<GenreDTO> GetByName(string name)
        {
            try
            {
                var genre = _genreRepo.GetByName(name);
                if (genre == null) return NotFound($"Genre \"{name}\" not found.");

                var dtoGenre = _mapper.Map<GenreDTO>(genre);
                return Ok(dtoGenre);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.GetByName", $"Error retrieving genre \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpPost("[action]")] // /api/Genre/Create
        public ActionResult<GenreDTO> Create([FromBody] string name)
        {
            try
            {
                var genre = _genreRepo.Create(name);
                if (genre == null) return Conflict($"Genre \"{name}\" already exists.");

                var dtoGenre = _mapper.Map<GenreDTO>(genre);
                return CreatedAtAction(nameof(Create), new { id = genre.Id }, dtoGenre);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Create", $"Error creating genre \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpPut("{name}/[action]")] // /api/Genre/{name}/Update
        public IActionResult Update(string name, [FromBody] string newName)
        {
            try
            {
                string result = _genreRepo.Update(name, newName);

                if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);

                if (result.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                    return Conflict(result);

                return NoContent();
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Update", $"Error updating genre \"{name}\": {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{name}/[action]")] // /api/Genre/{name}/Delete
        public IActionResult Delete(string name)
        {
            try
            {
                bool success = _genreRepo.Delete(name);
                if (!success) return NotFound($"Genre \"{name}\" not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Delete", $"Error deleting genre \"{name}\": {ex.Message}");
                throw;
            }
        }
    }
}
