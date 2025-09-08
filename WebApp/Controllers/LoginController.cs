using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using WebApp.ViewModels;
using BL.Repositories;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public LoginController(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new VMLogin());
        }

        [HttpPost]
        public IActionResult Login(VMLogin model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var secureKey = _config["JWT:SecureKey"];
            var loginResult = _userRepo.LoginUser(model.Name, model.Password, secureKey);

            if (string.IsNullOrEmpty(loginResult) || loginResult.StartsWith("The user exists"))
            {
                model.Message = string.IsNullOrEmpty(loginResult)
                    ? "User does not exist."
                    : loginResult;

                return View("Index", model);
            }
            return RedirectToAction("List", "Book");
        }

    }
}
