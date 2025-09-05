using System.Diagnostics;
using AutoMapper;
using BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var blUsers = _userRepo.GetAll();

        var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);

        return View(vmUsers);
    }

}
