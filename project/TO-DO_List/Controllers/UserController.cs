using System.Diagnostics;
using System.Web;
using BusinessLogicLayer.Service;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models;
using TO_DO_List.Models.User;

namespace TO_DO_List.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        public UserController(ILogger<UserController> logger, SignInManager<User> signInManager, IAccountService accountService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resultUser = await _accountService.CreateUserAsync(model.Login, model.FirstName, model.LastName,
                model.Email, model.Password, model.ConfirmPassword);

            if (resultUser == null)
            {
                ModelState.AddModelError("", "Логін уже зайнято. Або невірно введено пароль/його підтвердження.");
                return View(model);
            }
            _logger.LogInformation("User created a new account.");

            await _signInManager.SignInAsync(resultUser, isPersistent: false);
            return RedirectToAction("Index", "Tasks");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            var result = await _accountService.SignInAsync(model.Login, model.Password, model.RememberMe);

            if (result.Succeeded) return RedirectToAction("Index", "Tasks");

            ModelState.AddModelError("", "Неправильний логін або пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.ResetPasswordAsync(model.Login, model.Password);

            if (result.Succeeded) return View(model);
            ModelState.AddModelError("", "Невірно введені дані.");
            return View(model);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> CompleteResetPassword(string uid, string token, string newPassword)
        {
            var resetToken = HttpUtility.UrlDecode(token).Replace(' ', '+');

            await _accountService.CompleteResetPasswordAsync(uid, resetToken, newPassword);

            _logger.LogInformation("User updated an account with new password.");
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();
            return RedirectToAction("Login", "User");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}