using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepo;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepo, IMapper mapper)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
        }

        public IActionResult List()
        {
            var locations = _mapper.Map<IEnumerable<VMLocation>>(_locationRepo.GetLocations());
            return View(locations);
        }

        [HttpGet("/Location/[action]")]
        public IActionResult Add()
        {
            return View(new VMLocation());
        }

        [HttpPost("/Location/[action]")]
        public IActionResult Add(VMLocation model)
        {
            if (!ModelState.IsValid) return View(model);

            var created = _locationRepo.Create(model.Name);
            if (created == null)
            {
                ModelState.AddModelError("", "Location already exists.");
                return View(model);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet("/Location/[action]")]
        public IActionResult Update(int id, string name)
        {
            var vm = new VMLocation { Id = id, Name = name };
            return View(vm);
        }

        [HttpPost("/Location/[action]")]
        public IActionResult Update(string OriginalName, VMLocation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _locationRepo.Update(OriginalName, model.Name);

            if (result.Contains("already exists") || result.Contains("not found"))
            {
                ModelState.AddModelError("", result);
                return View(model);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet("/Location/[action]")]
        public IActionResult Delete(string name)
        {
            var success = _locationRepo.Delete(name);
            if (!success) TempData["Error"] = "Location not found.";
            return RedirectToAction(nameof(List));
        }
    }
}
