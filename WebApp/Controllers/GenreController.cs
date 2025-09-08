using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepo;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepo, IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public IActionResult List()
        {
            var genres = _mapper.Map<IEnumerable<VMGenre>>(_genreRepo.GetGenres());
            return View(genres);
        }

        [HttpGet("/Genre/[action]")]
        public IActionResult Add()
        {
            return View(new VMGenre());
        }

        [HttpPost("/Genre/[action]")]
        public IActionResult Add(VMGenre model)
        {
            if (!ModelState.IsValid) return View(model);

            var created = _genreRepo.Create(model.Name);
            if (created == null)
            {
                ModelState.AddModelError("", "Genre already exists.");
                return View(model);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet("/Genre/[action]")]
        public IActionResult Update(int id, string name)
        {
            var vm = new VMGenre { Id = id, Name = name };
            return View(vm);
        }

        [HttpPost("/Genre/[action]")]
        public IActionResult Update(string OriginalName, VMGenre model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _genreRepo.Update(OriginalName, model.Name);

            if (result.Contains("already exists") || result.Contains("not found"))
            {
                ModelState.AddModelError("", result);
                return View(model);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet("/Genre/[action]")]
        public IActionResult Delete(string name)
        {
            var success = _genreRepo.Delete(name);
            if (!success) TempData["Error"] = "Genre not found.";
            return RedirectToAction(nameof(List));
        }
    }
}
