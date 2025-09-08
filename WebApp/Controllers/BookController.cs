using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly RwaLibraryContext _dbContext;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepo, RwaLibraryContext dbContext, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult List(string? search, int? genreId, int page = 1, int pageSize = 10)
        {
            var result = _bookRepo.GetPaged(search, genreId, page, pageSize);

            var vmBooks = _mapper.Map<IEnumerable<VMBook>>(result.Items);

            var genres = _dbContext.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name,
                    Selected = (genreId.HasValue && g.Id == genreId.Value)
                })
                .ToList();

            var model = new VMBookList
            {
                Books = vmBooks,
                Search = search,
                SelectedGenreId = genreId,
                Genres = genres,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };

            return View(model);
        }

        [HttpGet("/Book/[action]")]
        public IActionResult Create()
        {
            ViewBag.Genres = _dbContext.Genres
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList();
            return View(new VMBook());
        }

        [HttpPost("/Book/[action]")]
        public IActionResult Create(VMBook model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _dbContext.Genres
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                    .ToList();
                return View(model);
            }

            var blBook = _mapper.Map<BLBook>(model);
            _bookRepo.Create(blBook);

            return RedirectToAction("List");
        }

        [HttpGet("/Book/[action]")]
        public IActionResult Update(string name)
        {
            var book = _bookRepo.GetByName(name);
            if (book == null) return RedirectToAction("List");

            ViewBag.Genres = _dbContext.Genres
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList();

            var model = _mapper.Map<VMBook>(book);
            return View(model);
        }

        [HttpPost("/Book/[action]")]
        public IActionResult Update(string OriginalName, VMBook model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _dbContext.Genres
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                    .ToList();
                return View(model);
            }

            var blBook = _mapper.Map<BLBook>(model);
            _bookRepo.Update(OriginalName, blBook);
            return RedirectToAction("List");
        }

        [HttpGet("/Book/[action]")]
        public IActionResult Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _bookRepo.Delete(name);
            }
            return RedirectToAction("List");
        }

    }
}
