using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogicLayer.Service.Impl;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<User?> CreateUserAsync(string login, string firstName, string lastName, string email,
        string password, string confirmPassword)
    {
        var user = new User { UserName = login, Email = email, FirstName = firstName, LastName = lastName };
        if (password != confirmPassword) return null;
        var result = await _userManager.CreateAsync(user, password);
        return !result.Succeeded ? null : user;
    }

    public async Task<SignInResult> SignInAsync(string login, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(login, password, rememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
        }

        return result;
    }


    public async Task<IdentityResult> ResetPasswordAsync(string login, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(login);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Неправильний логін." });

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        if (result.Succeeded)
        {
            await _emailService.SendResetPasswordEmail(user, resetToken, newPassword);
        }

        return result;
    }

    public async Task<IdentityResult> CompleteResetPasswordAsync(string userId, string resetToken, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Невірно введені дані." });

        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

        return result;
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logout.");
    }
}