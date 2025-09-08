using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using AutoMapper;
using BL.Repositories;
using BL.BLModels;


namespace WebApp.Controllers
{

    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public AdminController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var admin = _userRepo.GetAll().FirstOrDefault(u => u.RoleId == 2);
            if (admin == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var vm = _mapper.Map<VMUser>(admin);
            return View(vm);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateProfile([FromBody] VMAdminProfile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blUser = _mapper.Map<BLUser>(model);
            var result = _userRepo.UpdateProfile(blUser);

            if (result == "Success")
            {
                return Json(new { success = true, message = "Profile updated successfully." });
            }

            return Json(new { success = false, message = result });
        }

        [HttpPost("[action]")]
        public IActionResult UpdatePassword([FromBody] VMUser model)
        {
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest(new { success = false, message = "Password is required." });

            var result = _userRepo.UpdatePassword(model.Id, model.Password);

            if (result == "Success")
            {
                return Json(new { success = true, message = "Password updated successfully." });
            }

            return Json(new { success = false, message = result });
        }

    }
}
