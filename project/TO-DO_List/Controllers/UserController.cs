using System.Diagnostics;
using AutoMapper;
using BusinessLogicLayer.Service;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models;
using TO_DO_List.Models.User;

namespace TO_DO_List.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel model)
        {
            if (_userService.LoginUser(model.Login, model.Password))
            {
                return RedirectToAction("Index", "Tasks", new { userId = 1 });
            }

            ModelState.AddModelError("", "Неправильний логін або пароль");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var userFromDb = _userService.FindByLogin(model.Login);
            if (userFromDb != null)
            {
                ModelState.AddModelError("", "Користувач з таким логіном вже існує");
                return View(model);
            }

            if (!ModelState.IsValid) return View(model);
            var user = _mapper.Map<User>(model);
            _userService.RegisterUser(user);
            return RedirectToAction("Index", "Tasks", new { userId = 1 });

        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(RegisterViewModel model)
        {
            var userFromDb = _userService.FindByLogin(model.Login);
            if (userFromDb != null)
            {
                ModelState.AddModelError("", "Користувач з таким логіном вже існує");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                _userService.RegisterUser(user);
                return RedirectToAction("Index", "Tasks", new { userId = 1 });
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}