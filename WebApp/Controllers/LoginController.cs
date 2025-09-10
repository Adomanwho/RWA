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

            var user = _userRepo.CheckIfUserExists(model.Name);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
            }

            return RedirectToAction("List", "Book");
        }

        [HttpPost]
        public IActionResult Register(VMLogin model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            bool registered = _userRepo.RegisterUser(model.Name, model.Email, model.Password, 2);

            if (!registered)
            {
                model.Message = "User already exists or registration failed.";
                return View("Index", model);
            }

            model.Message = "Registration successful! You can now log in.";
            return View("Index", model);
        }

    }
}
