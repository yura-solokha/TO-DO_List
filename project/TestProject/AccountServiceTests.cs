namespace UnitTests;

using Microsoft.AspNetCore.Http;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AccountServiceTests
{
    private static readonly Mock<UserManager<User>> UserManagerMock = new(
        Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

    private readonly Mock<SignInManager<User>> _signInManagerMock = new(
        UserManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(),
        null, null, null, null);

    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<AccountService>> _loggerMock = new();
    private readonly IAccountService _accountService;

    public AccountServiceTests()
    {
        _accountService = new AccountService(
            UserManagerMock.Object, _signInManagerMock.Object, _emailServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsNull_IfPasswordsDoNotMatch()
    {
        const string login = "john.doe";
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john.doe@example.com";
        const string password = "Password123!";
        const string confirmPassword = "Password456!";

        var result =
            await _accountService.CreateUserAsync(login, firstName, lastName, email, password, confirmPassword);

        Assert.Null(result);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsNull_IfUserCreationFails()
    {
        const string login = "john.doe";
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john.doe@example.com";
        const string password = "Password123!";
        const string confirmPassword = "Password123!";

        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        var result =
            await _accountService.CreateUserAsync(login, firstName, lastName, email, password, confirmPassword);

        Assert.Null(result);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsUser_IfUserCreationSucceeds()
    {
        const string login = "john.doe";
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john.doe@example.com";
        const string password = "Password123!";
        const string confirmPassword = "Password123!";

        UserManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var result =
            await _accountService.CreateUserAsync(login, firstName, lastName, email, password, confirmPassword);

        Assert.NotNull(result);
        Assert.Equal(login, result.UserName);
        Assert.Equal(email, result.Email);
        Assert.Equal(firstName, result.FirstName);
        Assert.Equal(lastName, result.LastName);
    }

    [Fact]
    public async void SignInAsync_ValidParameters_ReturnsSuccess()
    {
        const string login = "tester";
        const string password = "TestPassword123";
        const bool rememberMe = false;
        _signInManagerMock.Setup(x =>
                x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        var result = await _accountService.SignInAsync(login, password, rememberMe);

        Assert.Equal(SignInResult.Success, result);
    }

    [Fact]
    public async void ResetPasswordAsync_ValidParameters_ReturnsSuccess()
    {
        const string login = "tester";
        const string newPassword = "NewTestPassword123";
        var user = new User { UserName = login, Email = "testuser@example.com" };
        UserManagerMock.Setup(x => x.FindByNameAsync(login)).ReturnsAsync(user);
        UserManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("resetToken");
        UserManagerMock.Setup(x => x.ResetPasswordAsync(user, "resetToken", newPassword))
            .ReturnsAsync(IdentityResult.Success);
        _emailServiceMock.Setup(x => x.SendResetPasswordEmail(user, "resetToken", newPassword)).Verifiable();

        var result = await _accountService.ResetPasswordAsync(login, newPassword);

        Assert.Equal(IdentityResult.Success, result);
        _emailServiceMock.Verify(x => x.SendResetPasswordEmail(user, "resetToken", newPassword), Times.Once());
    }
}