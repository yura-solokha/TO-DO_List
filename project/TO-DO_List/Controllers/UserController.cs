﻿using System.Diagnostics;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Impl;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IEmailService _emailService;

        public UserController(ILogger<UserController> logger,
            UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

            var user = new User
            {
                UserName = model.Login,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if(model.Password == model.ConfirmPassword) {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account.");
                    return RedirectToAction("Index", "Tasks");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogInformation("Error creating user: " + error.Description);
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToAction("Index", "Tasks");
            }

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

            var user = await _userManager.FindByNameAsync(model.Login);

            if (user == null)
            {
                ModelState.AddModelError("", "Невірно введені дані.");
                return View(model);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
 
            await _emailService.SendEmail(user, resetToken, model.Password);

            return View(model);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> CompleteResetPassword()
        {
            string uid = HttpContext.Request.Query["uid"];
            string resetToken = HttpContext.Request.Query["token"].ToString().Replace(' ', '+');
            string newPassword = HttpContext.Request.Query["newPassword"];

            var user = await _userManager.FindByIdAsync(uid);
            await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            
            _logger.LogInformation("User updated an account with new password.");
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logout.");
            return RedirectToAction("Login", "User");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}