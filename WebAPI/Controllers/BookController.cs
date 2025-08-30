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
    //ako zelis da je neki controller available bez tokena napisi [AllowAnonymous]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;
        private readonly RwaLibraryContext _dbContext;
        public BookController(IMapper mapper, IBookRepository bookRepo, RwaLibraryContext dbContext)
        {
            _mapper = mapper;
            _bookRepo = bookRepo;
            _dbContext = dbContext;
        }

        [HttpGet("[action]")]// /api/Book/GetPaged
        public ActionResult<PagedResult<BookDTO>> GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                PagedResult<BLBook> pagedResult = _bookRepo.GetPaged(search, page, pageSize);
                PagedResult<BookDTO> mappedResult = _mapper.Map<PagedResult<BookDTO>>(pagedResult);
                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.GetPaged", $"Error while getting paged result {ex.Message}");
                throw;
            }
        }

        [HttpGet("{id}/[action]")]// /api/Book/{id}/GetById
        public ActionResult<BookDTO> GetById(int id)
        {
            try
            {
                BLBook bLBook = _bookRepo.GetById(id);

                if (bLBook == null) return NotFound();

                BookDTO dtoBook = _mapper.Map<BookDTO>(bLBook);
                return Ok(dtoBook);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.GetById", $"Error while getting book from DB {ex.Message}");
                throw;
            }
        }

        [HttpPost("[action]")]// /api/Book/Create
        public ActionResult<BookDTO> Create(BookDTO book)
        {
            try
            {
                var bookToSend = _mapper.Map<BLBook>(book);
                var blBook = _bookRepo.Create(bookToSend);

                if (blBook == null) return NotFound("Genre not found.");

                var dtoBook = _mapper.Map<BookDTO>(blBook);

                return CreatedAtAction(nameof(Create), new { id = blBook.Id }, dtoBook);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}/[action]")]// /api/Book/{id}/Update
        public IActionResult Update(int id, [FromBody] BookDTO book)
        {
            //Malo cheesy nacin za napravit condition ali ok
            try
            {
                BLBook blBook = _mapper.Map<BLBook>(book);

                string returnedString = _bookRepo.Update(id, blBook);

                //this signifies something went wrong, look at the bookrepo code
                if (returnedString[0] == 'T') return NotFound("Please check the entered data again.");
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}/[action]")]// /api/Book/{id}/Delete
        public IActionResult Delete(int id)
        {
            try
            {
                bool returned = _bookRepo.Delete(id);
                if (!returned) return NotFound("No book with this id.");
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
