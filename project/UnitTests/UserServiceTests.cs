using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Moq;

namespace UnitTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _iUserRepositoryMock;
    private readonly Mock<IEntityRepository<User>> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IEntityRepository<User>>();
        _iUserRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object, _iUserRepositoryMock.Object);
    }

    [Fact]
    public void FindAll_ReturnsListOfUsers()
    {
        var users = new List<User> { new(), new() };
        _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(users.AsQueryable());

        var result = _userService.FindAll();

        Assert.Equal(users, result);
    }

    [Fact]
    public void FindById_ReturnsUserWithMatchingId()
    {
        var user = new User { Id = 1 };
        _userRepositoryMock.Setup(repo => repo.GetById(1)).Returns(user);

        var result = _userService.FindById(1);

        Assert.Equal(user, result);
    }

    [Fact]
    public void RegisterUser_CreatesNewUser()
    {
        var user = new User();

        _userService.RegisterUser(user);

        _userRepositoryMock.Verify(repo => repo.Create(user), Times.Once);
    }

    [Fact]
    public void LoginUser_ReturnsTrueIfLoginAndPasswordMatch()
    {
        const string login = "testuser";
        const string password = "testpassword";
        var user = new User { Login = login, Password = password };

        _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<User> { user }.AsQueryable());

        var result = _userService.LoginUser(login, password);

        Assert.True(result);
    }

    [Fact]
    public void LoginUser_ReturnsFalseIfUserNotFound()
    {
        const string login = "testuser";
        const string password = "testpassword";
        _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<User>().AsQueryable());

        var result = _userService.LoginUser(login, password);

        Assert.False(result);
    }

    [Fact]
    public void Delete_RemovesUserWithMatchingId()
    {
        const int id = 1;

        _userService.Delete(id);

        _userRepositoryMock.Verify(repo => repo.Delete(id), Times.Once);
    }

    [Fact]
    public void FindByLogin_ReturnsUserWithMatchingLogin()
    {
        const string login = "testuser";
        var user = new User { Login = login };
        _iUserRepositoryMock.Setup(repo => repo.GetByLogin(login)).Returns(user);

        var result = _userService.FindByLogin(login);

        Assert.Equal(user, result);
    }
}