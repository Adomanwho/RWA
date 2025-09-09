using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class LocationBookController : Controller
    {
        private readonly ILocationBookRepository _repo;
        private readonly IMapper _mapper;

        public LocationBookController(ILocationBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IActionResult List()
        {
            var items = _repo.GetAll();
            var vm = _mapper.Map<IEnumerable<VMLocationBook>>(items);
            return View(vm);
        }

        [HttpGet("/LocationBook/[action]")]
        public IActionResult Add() => View(new VMLocationBook());

        [HttpPost("/LocationBook/[action]")]
        public IActionResult Add(VMLocationBook model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                _repo.Create(model.BookName, model.LocationName);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                return View(model);
            }
        }

        [HttpGet("/LocationBook/[action]")]
        public IActionResult Update(string bookName, string locationName)
        {
            var entity = _repo.GetAll()
                .FirstOrDefault(lb => lb.Book.Name == bookName && lb.Location.Name == locationName);

            if (entity == null)
                return RedirectToAction("List");

            var vm = _mapper.Map<VMLocationBook>(entity);
            return View(vm);
        }

        [HttpPost("/LocationBook/[action]")]
        public IActionResult Update(string originalBookName, string originalLocationName, VMLocationBook model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = _repo.Update(originalBookName, originalLocationName, model.BookName, model.LocationName);
            if (result == "Success") return RedirectToAction("List");

            model.Message = result;
            return View(model);
        }

        [HttpPost("/LocationBook/[action]")]
        public IActionResult Delete(string bookName, string locationName)
        {
            _repo.Delete(bookName, locationName);
            return RedirectToAction("List");
        }
    }
}
