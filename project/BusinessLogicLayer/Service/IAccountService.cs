using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogicLayer.Service;

public interface IAccountService
{
    Task<User?> CreateUserAsync(string login, string firstName, string lastName, string email,
        string password, string confirmPassword);

    Task<SignInResult> SignInAsync(string login, string password, bool rememberMe);

    Task<IdentityResult> ResetPasswordAsync(string login, string newPassword);

    Task<IdentityResult> CompleteResetPasswordAsync(string userId, string resetToken, string newPassword);

    Task SignOut();
}